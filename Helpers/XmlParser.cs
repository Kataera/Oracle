/*
    #################
    ##   License   ##
    #################

    Tarot - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Tarot.

    Tarot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Tarot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Tarot. If not, see http://www.gnu.org/licenses/.
*/

namespace Tarot.Helpers
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    using global::Tarot.Data;
    using global::Tarot.Data.FateTypes;
    using global::Tarot.Enumerations;

    internal static class XmlParser
    {
        private static bool fateDataInvalidFlag;

        private static FateDatabase database;

        private static XmlDocument fateDataXml;

        private static string fateName;

        private static uint fateId;

        private static uint fateLevel;

        private static FateType fateType;

        private static FateSupportLevel fateSupportLevel;

        private static uint fateCollectItemId;

        private static uint fateNpcId;

        private static uint fateChainIdSuccess;

        private static uint fateChainIdFail;

        private static Fate CreateFate()
        {
            Fate fate;
            switch (fateType)
            {
                case FateType.Kill:
                    fate = new Kill();
                    break;

                case FateType.Collect:
                    fate = new Collect();
                    break;

                case FateType.Escort:
                    fate = new Escort();
                    break;

                case FateType.Defence:
                    fate = new Defence();
                    break;

                case FateType.Boss:
                    fate = new Boss();
                    break;

                case FateType.MegaBoss:
                    fate = new MegaBoss();
                    break;

                default:
                    Logger.SendErrorLog("Error during FATE data parse.");
                    Logger.SendDebugLog("FATE type is undefined, check FATE data is parsing correctly.");
                    return null;
            }

            fate.ChainIdFailure = fateChainIdFail;
            fate.ChainIdSuccess = fateChainIdSuccess;
            fate.Id = fateId;
            fate.ItemId = fateCollectItemId;
            fate.Level = fateLevel;
            fate.Name = fateName;
            fate.NpcId = fateNpcId;
            fate.SupportLevel = fateSupportLevel;
            fate.Type = fateType;
            return fate;
        }

        public static FateDatabase GetFateDatabase()
        {
            if (database == null)
            {
                ParseFateData();
            }

            return database;
        }

        public static FateDatabase GetFateDatabase(bool forceReparse)
        {
            if (forceReparse || database == null)
            {
                ParseFateData();
            }

            return database;
        }

        private static XmlDocument GetXmlDocument()
        {
            fateDataInvalidFlag = false;
            var xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(Environment.CurrentDirectory + "\\BotBases\\Tarot\\Data\\FateData.xml");
                xmlDocument.Schemas.Add(null, Environment.CurrentDirectory + "\\BotBases\\Tarot\\Data\\FateData.xsd");
                xmlDocument.Validate(ValidationEventHandler);
            }
            catch (DirectoryNotFoundException exception)
            {
                Logger.SendErrorLog("Directory structure is incorrect, did you place Tarot in the correct place?");
                Logger.SendDebugLog("DirectoryNotFoundException thrown.\n\n" + exception + "\n");
            }
            catch (IOException exception)
            {
                Logger.SendErrorLog("Cannot find xml data, did you place Tarot in the correct place?");
                Logger.SendDebugLog("IOException thrown.\n\n" + exception + "\n");
            }

            return xmlDocument;
        }

        private static void ParseFateData()
        {
            fateDataXml = GetXmlDocument();
            database = new FateDatabase();

            // Parse fate data.
            if (!fateDataInvalidFlag && fateDataXml.DocumentElement != null)
            {
                try
                {
                    // Parse each node.
                    foreach (XmlNode currentNode in fateDataXml.DocumentElement.ChildNodes)
                    {
                        // Ensure nodes are instantiated.
                        fateId = 0;
                        fateName = string.Empty;
                        fateLevel = 0;
                        fateType = FateType.Null;
                        fateSupportLevel = FateSupportLevel.Unsupported;
                        fateCollectItemId = 0;
                        fateNpcId = 0;
                        fateChainIdSuccess = 0;
                        fateChainIdFail = 0;

                        if (currentNode["ID"] != null)
                        {
                            fateId = uint.Parse(currentNode["ID"].InnerText);
                        }

                        if (currentNode["Name"] != null)
                        {
                            fateName = currentNode["Name"].InnerText;
                        }

                        if (currentNode["Level"] != null)
                        {
                            fateLevel = uint.Parse(currentNode["Level"].InnerText);
                        }

                        if (currentNode["Type"] != null)
                        {
                            fateType = (FateType) int.Parse(currentNode["Type"].InnerText);
                        }

                        if (currentNode["TarotSupport"] != null)
                        {
                            fateSupportLevel = (FateSupportLevel) int.Parse(currentNode["TarotSupport"].InnerText);
                        }

                        if (currentNode["CollectItemId"] != null)
                        {
                            fateCollectItemId = uint.Parse(currentNode["CollectItemId"].InnerText);
                        }

                        if (currentNode["NpcId"] != null)
                        {
                            fateNpcId = uint.Parse(currentNode["NpcId"].InnerText);
                        }

                        if (currentNode["ChainIDSuccess"] != null)
                        {
                            fateChainIdSuccess = uint.Parse(currentNode["ChainIDSuccess"].InnerText);
                        }

                        if (currentNode["ChainIDFailure"] != null)
                        {
                            fateChainIdFail = uint.Parse(currentNode["ChainIDFailure"].InnerText);
                        }

                        database.AddFateToDatabase(CreateFate());
                    }
                }
                catch (FormatException exception)
                {
                    Logger.SendErrorLog("Formatting error during fate data parse.");
                    Logger.SendDebugLog("FormatException thrown:\n\n" + exception + "\n");
                }
                catch (OverflowException exception)
                {
                    Logger.SendErrorLog("Numerical conversion resulted in overflow.");
                    Logger.SendDebugLog("OverflowException thrown.\n\n" + exception + "\n");
                }
            }
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            Logger.SendDebugLog("ValidationEvent occurred.");

            if (args.Severity == XmlSeverityType.Warning)
            {
                // Assume warning is a missing XML schema file.
                Logger.SendErrorLog("XML schema file could not be found. Check that .\\Data\\FateData.xsd exists.");
                fateDataInvalidFlag = true;
            }
            else
            {
                Logger.SendErrorLog(
                    "Fate data failed to validate, this is normally caused by a modified FateData.xml file.");
                Logger.SendDebugLog("Fate data validation failure:\n\n" + args.Message + "\n");
                fateDataInvalidFlag = true;
            }
        }
    }
}