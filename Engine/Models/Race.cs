using System.Collections.Generic;
namespace Engine.Models
{
    public class Race
    {
        public string Key { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<PlayerAttributeModifier> PlayerAttributeModifiers { get; } =
            new List<PlayerAttributeModifier>();
    }
}