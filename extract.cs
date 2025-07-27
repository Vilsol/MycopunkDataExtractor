var filePath = "upgrades.json";

var upgradeMap = new SortedDictionary<int, string>();

foreach (var upgradable in Global.Instance.AllGear.Concat(Global.Instance.Characters))
{
    Log(upgradable);
    foreach  (var up in upgradable.Info.Upgrades)
    {
        var sb = new StringBuilder();

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

        if (up.Properties.properties != null)
        {
            sb.Append($",\"Properties\":[");
            for (var j = 0; j < up.Properties.properties.Length; j++)
            {
                if (j != 0) {
                    sb.Append(",");
                }
                
                sb.Append("{");
                sb.Append($"\"Type\":\"{up.Properties.properties[j].ToString()}\"");
                sb.Append($", \"Raw\":{JsonUtility.ToJson(up.Properties.properties[j])}");

                var stats = up.Properties.properties[j].GetStatData(new Pigeon.Math.Random(), upgradable);
                if (stats != null)
                {
                    var firstStat = true;
                    sb.Append($",\"Stats\":[");
                    while (stats.MoveNext())
                    {
                        if (!firstStat) {
                            sb.Append(",");
                        }
                        firstStat = false;
                        
                        var stat = stats.Current;
                        sb.Append(JsonUtility.ToJson(stat));
                    }
                    sb.Append($"]");
                }

                sb.Append("}");
            }
            sb.Append($"]");
        }

        sb.Append($",\"Pattern\":{JsonUtility.ToJson(up.Pattern)}");

        sb.Append($",\"UnlockCost\":[");
        var unlockCost = up.GetUnlockCost();
        for (var j = 0; j < unlockCost.Count; j++)
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
            for (var j = 0; j < additionalUnlockCost.Length; j++)
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

        upgradeMap.TryAdd(up.ID, sb.ToString());
    }
}

using (var writer = new System.IO.StreamWriter(filePath, false))
{
    writer.Write("[\n");

    var first = true;
    foreach (var kvp in upgradeMap)
    {        
        if (!first) {
            writer.Write(",\n");
        }
        first = false;

        writer.Write(kvp.Value);
    }

    writer.Write("\n]");
}
