using System;
using MonoGame.Extended.TextureAtlases;
using System.Collections.Generic;
using MonoGame.Extended.Collections;

namespace MonoGame.Extended.BitmapFonts
{
    public class BitmapFontRegion
    {
        public int Character { get; }
        public TextureRegion2D TextureRegion { get; }
        public int XOffset { get; }
        public int YOffset { get; }
        public int XAdvance { get; }
        public float Width => TextureRegion.Width;
        public float Height => TextureRegion.Height;

        public Dictionary<int, int> Kernings { get; }

        public BitmapFontRegion(
            TextureRegion2D textureRegion, int character, int xOffset, int yOffset, int xAdvance)
        {
            TextureRegion = textureRegion;
            Character = character;
            XOffset = xOffset;
            YOffset = yOffset;
            XAdvance = xAdvance;
            Kernings = new Dictionary<int, int>(1);
        }


        public override string ToString()
        {
            return $"{Convert.ToChar(Character)} {TextureRegion}";
        }
    }
}