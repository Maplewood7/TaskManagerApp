namespace WebApp.Components
{
    public static class TagColorStyle
    {
        public static string Normalize(string? color)
        {
            if (color is null ||
                color.Length != 7 ||
                color[0] != '#' ||
                !color.Skip(1).All(Uri.IsHexDigit))
            {
                return "#6258E8";
            }

            return color.ToUpperInvariant();
        }

        public static string BadgeStyle(string? color)
        {
            var normalizedColor = Normalize(color);
            var red = Convert.ToInt32(normalizedColor.Substring(1, 2), 16);
            var green = Convert.ToInt32(normalizedColor.Substring(3, 2), 16);
            var blue = Convert.ToInt32(normalizedColor.Substring(5, 2), 16);
            var brightness = (red * 299 + green * 587 + blue * 114) / 1000;
            var foreground = brightness >= 150 ? "#172033" : "#FFFFFF";

            return $"--tag-color: {normalizedColor}; --tag-foreground: {foreground};";
        }
    }
}