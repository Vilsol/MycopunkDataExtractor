using System.IO;

GenericGunUpgrade[] allUpgrades = Resources.FindObjectsOfTypeAll<GenericGunUpgrade>();
Log("Found " + allUpgrades.Length + " upgrades");

string filePath = "upgrades.json";
try
{
    using (StreamWriter writer = new StreamWriter(filePath, false))
    {
        writer.Write("[\n");

        for (int i = 0; i < allUpgrades.Length; i++)
        {
            StringBuilder sb = new StringBuilder();

            if (i != 0) {
                sb.Append(",\n");
            }

            GenericGunUpgrade up = allUpgrades[i];

            sb.Append("{");

            sb.Append($"\"ID\":{up.ID}");
            sb.Append($",\"APIName\":\"{up.APIName}\"");
            sb.Append($",\"Name\":\"{up.Name.Replace("\"", "\\\"")}\"");
            sb.Append($",\"EffectType\":\"{up.EffectType}\"");
            sb.Append($",\"UpgradeType\":\"{up.UpgradeType}\"");
            sb.Append($",\"Rarity\":\"{up.Rarity}\"");
            sb.Append($",\"Flags\":\"{up.Flags}\"");
            sb.Append($",\"Color\":\"{up.Color}\"");
            sb.Append($",\"MustBeUnlockedFirst\":\"{up.MustBeUnlockedFirst}\"");
            sb.Append($",\"Description\":\"{up.Description.Replace("\r\n", "\\n")}\"");
            sb.Append($",\"Properties\":{JsonUtility.ToJson(up.Properties)}");
            sb.Append($",\"Pattern\":{JsonUtility.ToJson(up.Pattern)}");

            sb.Append($",\"UnlockCost\":[");
            var unlockCost = up.GetUnlockCost();
            for (int j = 0; j < unlockCost.Count; j++)
            {
                if (j != 0) {
                    sb.Append(",");
                }
                
                sb.Append("{");
                sb.Append($"\"Count\":\"{unlockCost[j].count}\"");
                sb.Append($",\"Resource\":\"{unlockCost[j].resource.Name}\"");
                sb.Append("}");
            }
            sb.Append($"]");

            sb.Append($",\"AdditionalUnlockCost\":[");
            if (up.additionalUnlockCost != null)
            {
                var additionalUnlockCost = up.additionalUnlockCost;
                for (int j = 0; j < additionalUnlockCost.Length; j++)
                {
                    if (j != 0) {
                        sb.Append(",");
                    }
                    
                    sb.Append("{");
                    sb.Append($"\"Count\":\"{additionalUnlockCost[j].count}\"");
                    sb.Append($",\"Resource\":\"{additionalUnlockCost[j].resource.Name}\"");
                    sb.Append("}");
                }
            }
            sb.Append($"]");

            sb.Append("}");

            string json = sb.ToString();
            
            writer.Write(json);

            // Log("Wrote: " + up.Name);
        }

        writer.Write("\n]");
    }
    Log("Successfully wrote " + allUpgrades.Length + " upgrades to " + filePath);
}
catch (System.Exception e)
{
    Log("An error occurred: " + e.Message);
}
