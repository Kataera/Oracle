using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

using ff14bot.Settings;

using Oracle.Data.Fates;
using Oracle.Enumerations;
using Oracle.Structs;

namespace Oracle.Helpers
{
    internal static class XmlParser
    {
        private static FateDatabase database;
        private static uint fateChainId;
        private static uint fateCollectItemId;
        private static bool fateDataInvalidFlag;
        private static XmlDocument fateDataXml;
        private static uint fateId;
        private static float fateLandingRadius;
        private static uint fateLevel;
        private static string fateName;
        private static uint fateNpcId;
        private static List<uint> fatePreferredTargetId;
        private static FateSupportLevel fateSupportLevel;
        private static FateType fateType;

        private static Fate CreateFate()
        {
            var fate = new Fate
            {
                ChainId = fateChainId,
                Id = fateId,
                ItemId = fateCollectItemId,
                LandingRadius = fateLandingRadius,
                Level = fateLevel,
                Name = fateName,
                NpcId = fateNpcId,
                PreferredTargetId = fatePreferredTargetId,
                SupportLevel = fateSupportLevel,
                Type = fateType
            };

            return fate;
        }

        internal static async Task<FateDatabase> GetFateDatabase()
        {
            if (database == null)
            {
                await ParseFateData();
            }

            return database;
        }

        internal static async Task<FateDatabase> GetFateDatabase(bool forceReparse)
        {
            if (forceReparse || database == null)
            {
                await ParseFateData();
            }

            return database;
        }

        private static XmlDocument GetXmlDocument()
        {
            fateDataInvalidFlag = false;
            var xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(GlobalSettings.Instance.BotBasePath + @"\Oracle\Data\Fates\FateData.xml");
                xmlDocument.Schemas.Add(null, GlobalSettings.Instance.BotBasePath + @"\Oracle\Data\Fates\FateData.xsd");
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

        private static async Task ParseFateData()
        {
            fateDataXml = GetXmlDocument();
            database = new FateDatabase();

            if (fateDataInvalidFlag || fateDataXml.DocumentElement == null)
            {
                return;
            }

            try
            {
                foreach (XmlNode currentNode in fateDataXml.DocumentElement.ChildNodes)
                {
                    // Ensure nodes are instantiated.
                    fateId = 0;
                    fateName = string.Empty;
                    fateLevel = 0;
                    fateType = FateType.Null;
                    fateSupportLevel = FateSupportLevel.Unsupported;
                    fateLandingRadius = 1f;
                    fateCollectItemId = 0;
                    fateNpcId = 0;
                    fatePreferredTargetId = new List<uint>();
                    fateChainId = 0;

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

                    if (currentNode["LandingRadius"] != null)
                    {
                        fateLandingRadius = float.Parse(currentNode["LandingRadius"].InnerText);
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

                    if (currentNode["ChainID"] != null)
                    {
                        fateChainId = uint.Parse(currentNode["ChainID"].InnerText);
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
                Logger.SendErrorLog("Fate data failed to validate, this is normally caused by a modified FateData.xml file.");
                Logger.SendDebugLog("Fate data validation failure:\n\n" + args.Message + "\n");
                fateDataInvalidFlag = true;
            }
        }
    }
}