/*
    #################
    ##   License   ##
    #################

    Oracle - An improved FATE bot for RebornBuddy
    Copyright © 2015 Caitlin Howarth (a.k.a. Kataera)

    This file is part of Oracle.

    Oracle is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Oracle is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Oracle. If not, see http://www.gnu.org/licenses/.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

using Clio.Utilities;

using Oracle.Data;
using Oracle.Enumerations;

namespace Oracle.Helpers
{
    internal static class XmlParser
    {
        private static OracleDatabase database;
        private static uint fateChainIdFail;
        private static uint fateChainIdSuccess;
        private static uint fateCollectItemId;
        private static List<Waypoint> fateCustomWaypoints;
        private static bool fateDataInvalidFlag;
        private static XmlDocument fateDataXml;
        private static uint fateId;
        private static uint fateLevel;
        private static string fateName;
        private static uint fateNpcId;
        private static List<uint> fatePreferredTargetId;
        private static FateSupportLevel fateSupportLevel;
        private static FateType fateType;
        private static uint? waypointOrder;
        private static float? waypointXCoord;
        private static float? waypointYCoord;
        private static float? waypointZCoord;

        public static OracleDatabase GetFateDatabase()
        {
            if (database == null)
            {
                ParseFateData();
            }

            return database;
        }

        public static OracleDatabase GetFateDatabase(bool forceReparse)
        {
            if (forceReparse || database == null)
            {
                ParseFateData();
            }

            return database;
        }

        private static Fate CreateFate()
        {
            var fate = new Fate
            {
                ChainIdFailure = fateChainIdFail,
                ChainIdSuccess = fateChainIdSuccess,
                Id = fateId,
                ItemId = fateCollectItemId,
                Level = fateLevel,
                Name = fateName,
                NpcId = fateNpcId,
                PreferredTargetId = fatePreferredTargetId,
                SupportLevel = fateSupportLevel,
                Type = fateType,
                CustomWaypoints = fateCustomWaypoints
            };

            return fate;
        }

        private static Waypoint CreateWaypoint()
        {
            var waypoint = new Waypoint
            {
                Order = waypointOrder ?? default(uint),
                Location = new Vector3(waypointXCoord ?? default(float), waypointYCoord ?? default(float), waypointZCoord ?? default(float))
            };

            return waypoint;
        }

        private static XmlDocument GetXmlDocument()
        {
            fateDataInvalidFlag = false;
            var xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(Environment.CurrentDirectory + "\\BotBases\\Oracle\\Data\\FateData.xml");
                xmlDocument.Schemas.Add(null, Environment.CurrentDirectory + "\\BotBases\\Oracle\\Data\\FateData.xsd");
                xmlDocument.Validate(ValidationEventHandler);
            }
            catch (DirectoryNotFoundException exception)
            {
                Logger.SendErrorLog("Directory structure is incorrect, did you place Oracle in the correct place?");
                Logger.SendDebugLog("DirectoryNotFoundException thrown.\n\n" + exception);
            }
            catch (IOException exception)
            {
                Logger.SendErrorLog("Cannot find xml data, did you place Oracle in the correct place?");
                Logger.SendDebugLog("IOException thrown.\n\n" + exception);
            }

            return xmlDocument;
        }

        private static void ParseFateData()
        {
            fateDataXml = GetXmlDocument();
            database = new OracleDatabase();

            if (fateDataInvalidFlag || fateDataXml.DocumentElement == null)
            {
                return;
            }

            // Parse fate data.
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
                    fatePreferredTargetId = new List<uint>();
                    fateChainIdSuccess = 0;
                    fateChainIdFail = 0;
                    fateCustomWaypoints = new List<Waypoint>();

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

                    if (currentNode["OracleSupport"] != null)
                    {
                        fateSupportLevel = (FateSupportLevel) int.Parse(currentNode["OracleSupport"].InnerText);
                    }

                    if (currentNode["CollectItemId"] != null)
                    {
                        fateCollectItemId = uint.Parse(currentNode["CollectItemId"].InnerText);
                    }

                    if (currentNode["NpcId"] != null)
                    {
                        fateNpcId = uint.Parse(currentNode["NpcId"].InnerText);
                    }

                    if (currentNode["PreferredTargetId"] != null)
                    {
                        var targets = currentNode.SelectNodes("PreferredTargetId");
                        if (targets != null)
                        {
                            for (var i = 0; i < targets.Count; i++)
                            {
                                var xmlNode = targets.Item(i);
                                if (xmlNode != null)
                                {
                                    fatePreferredTargetId.Add(uint.Parse(xmlNode.InnerText));
                                }
                            }
                        }
                    }

                    if (currentNode["ChainIDSuccess"] != null)
                    {
                        fateChainIdSuccess = uint.Parse(currentNode["ChainIDSuccess"].InnerText);
                    }

                    if (currentNode["ChainIDFailure"] != null)
                    {
                        fateChainIdFail = uint.Parse(currentNode["ChainIDFailure"].InnerText);
                    }

                    if (currentNode["Waypoints"] != null)
                    {
                        var waypointNodes = currentNode.SelectSingleNode("Waypoints");
                        if (waypointNodes != null)
                        {
                            foreach (XmlNode waypointNode in waypointNodes.ChildNodes)
                            {
                                waypointOrder = null;
                                waypointXCoord = null;
                                waypointYCoord = null;
                                waypointZCoord = null;

                                if (waypointNode["Order"] != null)
                                {
                                    waypointOrder = uint.Parse(waypointNode["Order"].InnerText);
                                }

                                if (waypointNode["X"] != null)
                                {
                                    waypointXCoord = float.Parse(waypointNode["X"].InnerText);
                                }

                                if (waypointNode["Y"] != null)
                                {
                                    waypointYCoord = float.Parse(waypointNode["Y"].InnerText);
                                }

                                if (waypointNode["Z"] != null)
                                {
                                    waypointZCoord = float.Parse(waypointNode["Z"].InnerText);
                                }

                                if (waypointOrder != null && waypointXCoord != null && waypointYCoord != null && waypointZCoord != null)
                                {
                                    fateCustomWaypoints.Add(CreateWaypoint());
                                }
                            }
                        }
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
                Logger.SendDebugLog("OverflowException thrown.\n\n" + exception);
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