﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Glyph = MonoGame.Extended.BitmapFonts.BitmapFont.Glyph;

namespace MonoGame.Extended.BitmapFonts
{
    public static partial class BitmapFontExtensions
    {
        public static SizeF GetGlyphSprites(
            this BitmapFont font, ICollection<GlyphSprite> output, string text, Vector2 position,
            Color color, float rotation, Vector2 origin, Vector2 scale, float depth, RectangleF? clipRect)
        {
            using (var glyphs = (GlyphEnumerator)font.GetGlyphs(text, position))
                return GetSprites(glyphs, output, position, color, rotation, origin, scale, depth, clipRect);
        }

        public static SizeF GetGlyphSprites(
            this BitmapFont font, ICollection<GlyphSprite> output, StringBuilder text, Vector2 position,
            Color color, float rotation, Vector2 origin, Vector2 scale, float depth, RectangleF? clipRect)
        {
            using (var glyphs = (GlyphEnumerator)font.GetGlyphs(text, position))
                return GetSprites(glyphs, output, position, color, rotation, origin, scale, depth, clipRect);
        }

        public static SizeF GetGlyphSprites(
            this BitmapFont font, ICollection<GlyphSprite> output, ICharIterator text, Vector2 position,
            Color color, float rotation, Vector2 origin, Vector2 scale, float depth, RectangleF? clipRect)
        {
            using (var glyphs = (GlyphEnumerator)font.GetGlyphs(text, position))
                return GetSprites(glyphs, output, position, color, rotation, origin, scale, depth, clipRect);
        }

        private static SizeF GetSprites(
            GlyphEnumerator glyphs, ICollection<GlyphSprite> output, Vector2 position,
            Color color, float rotation, Vector2 origin, Vector2 scale, float depth, RectangleF? clipRect)
        {
            GlyphSprite sprite;
            SizeF size = SizeF.Empty;

            int index = 0;
            while (glyphs.MoveNext())
            {
                Glyph glyph = glyphs.CurrentGlyph;
                if (glyph.FontRegion == null)
                    continue;

                Vector2 glyphOrigin = position - glyph.Position + origin;
                Vector2 newPos = position;
                TextureRegion2D region = glyph.FontRegion.TextureRegion;

                sprite.Visible = region.Bounds.IsVisible(
                    ref newPos, glyphOrigin, scale, clipRect, out RectangleF srcRect);

                if (!sprite.Visible) // restore values
                {
                    newPos = position;
                    srcRect = region.Bounds;
                }

                sprite.Char = glyph.Character;
                sprite.Texture = region.Texture;
                sprite.Index = index;
                sprite.SourceRect = srcRect;
                sprite.Position = newPos;
                sprite.Color = color;
                sprite.Rotation = rotation;
                sprite.Origin = glyphOrigin;
                sprite.Scale = scale;
                sprite.Depth = depth;

                output.Add(sprite);
                index++;

                float rowX = glyph.Position.X + srcRect.Width * scale.X - newPos.X;
                if (rowX > size.Width)
                    size.Width = rowX;

                float rowY = glyph.Position.Y + srcRect.Height * scale.Y - newPos.Y;
                if (rowY > size.Height)
                    size.Height = rowY;
            }
            return new SizeF(size.Width, size.Height);
        }

        public static SizeF GetGlyphBatchedSprites(
            this BitmapFont font, ICollection<GlyphBatchedSprite> output, string text, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2? scale, float depth, RectangleF? clipRect)
        {
            using (var glyphs = (GlyphEnumerator)font.GetGlyphs(text, position))
                return GetGlyphBatchedSprites(glyphs, output, position, color, rotation, origin, scale, depth, clipRect);
        }

        public static SizeF GetGlyphBatchedSprites(
            this BitmapFont font, ICollection<GlyphBatchedSprite> output, StringBuilder text, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2? scale, float depth, RectangleF? clipRect)
        {
            using (var glyphs = (GlyphEnumerator)font.GetGlyphs(text, position))
                return GetGlyphBatchedSprites(glyphs, output, position, color, rotation, origin, scale, depth, clipRect);
        }

        private static SizeF GetGlyphBatchedSprites(
            GlyphEnumerator glyphs, ICollection<GlyphBatchedSprite> output, Vector2 position,
            Color color, float rotation, Vector2 origin, Vector2? scale, float depth, RectangleF? clipRect)
        {
            GlyphBatchedSprite sprite;
            var size = new SizeF();
            var scaleValue = scale ?? Vector2.One;
            int index = 0;
            while (glyphs.MoveNext())
            {
                Glyph glyph = glyphs.CurrentGlyph;
                if (glyph.FontRegion == null)
                    continue;

                Vector2 glyphOrigin = position - glyph.Position + origin;
                Vector2 newPos = position;
                TextureRegion2D region = glyph.FontRegion.TextureRegion;

                sprite.Visible = region.Bounds.IsVisible(
                    ref newPos, glyphOrigin, scaleValue, clipRect, out RectangleF srcRect);

                if (!sprite.Visible) // restore values
                {
                    newPos = position;
                    srcRect = region.Bounds;
                }

                sprite.Char = glyph.Character;
                sprite.Index = index;
                sprite.Texture = region.Texture;
                sprite.Sprite = default;
                sprite.Sprite.SetTransform(newPos, rotation, scale, origin, srcRect.Size);
                sprite.Sprite.SetTexCoords(sprite.Texture.Texel, srcRect);
                sprite.Sprite.SetDepth(depth);
                sprite.Sprite.SetColor(color);

                output.Add(sprite);
                index++;

                float rowX = glyph.Position.X + srcRect.Width * scaleValue.X - newPos.X;
                if (rowX > size.Width)
                    size.Width = rowX;

                float rowY = glyph.Position.Y + srcRect.Height * scaleValue.Y - newPos.Y;
                if (rowY > size.Height)
                    size.Height = rowY;
            }
            return new SizeF(size.Width, size.Height);
        }

        [DebuggerHidden]
        private static void ThrowOnArgs(SpriteEffects effect)
        {
            if (effect != SpriteEffects.None)
                throw new NotSupportedException($"{effect} is currently not supported for {nameof(BitmapFont)}");
        }

        /// <summary>
        /// Adds a string to a batch of sprites for rendering using the specified font,
        /// text, position, color, rotation, origin, scale, effects and layer.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font">A font for displaying text.</param>
        /// <param name="text">The text message to display.</param>
        /// <param name="position">The location (in screen coordinates) to draw the text.</param>
        /// <param name="color">The <see cref="Color" /> to tint a sprite.</param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the text about its origin.</param>
        /// <param name="origin">The origin for each letter; the default is (0,0) which is the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effect">Effects to apply.</param>
        /// <param name="layerDepth">
        /// The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer.
        /// Use SpriteSortMode if you want sprites to be sorted during drawing.
        /// </param>
        /// <param name="clippingRectangle">
        /// Clips the boundaries of the text so that it's not drawn outside the clipping rectangle.
        /// </param>
        public static void DrawString(
            this SpriteBatch spriteBatch, BitmapFont font, string text, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float layerDepth, RectangleF? clippingRectangle = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            ThrowOnArgs(effect);

            using (var glyphs = font.GetGlyphs(text, position))
                DrawString(spriteBatch, glyphs, position, color, rotation, origin, scale, layerDepth, clippingRectangle);
        }

        public static void DrawString(
            this SpriteBatch spriteBatch, BitmapFont font, StringBuilder text, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float layerDepth, RectangleF? clippingRectangle = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            ThrowOnArgs(effect);

            using (var glyphs = font.GetGlyphs(text, position))
                DrawString(spriteBatch, glyphs, position, color, rotation, origin, scale, layerDepth, clippingRectangle);
        }

        private static void DrawString(
            SpriteBatch batch, IEnumerator<Glyph> glyphs, Vector2 position, Color color,
            float rotation, Vector2 origin, Vector2 scale, float depth, RectangleF? clipRect)
        {
            while (glyphs.MoveNext())
            {
                Glyph glyph = glyphs.Current;
                if (glyph.FontRegion == null)
                    continue;

                Vector2 characterOrigin = position - glyph.Position + origin;
                batch.Draw(
                    glyph.FontRegion.TextureRegion, position, color,
                    rotation, characterOrigin, scale, SpriteEffects.None, depth, clipRect);
            }
        }

        /// <summary>
        ///     Adds a string to a batch of sprites for rendering using the specified font, text, position, color,
        ///     rotation, origin, scale, effects and layer.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font">A font for displaying text.</param>
        /// <param name="text">The text message to display.</param>
        /// <param name="position">The location (in screen coordinates) to draw the text.</param>
        /// <param name="color">
        /// The <see cref="Color" /> to tint a sprite. Use <see cref="Color.White" /> for full color with no tinting.
        /// </param>
        /// <param name="rotation">Specifies the angle (in radians) to rotate the text about its origin.</param>
        /// <param name="origin">The origin for each letter; the default is (0,0) which is the upper-left corner.</param>
        /// <param name="scale">Scale factor.</param>
        /// <param name="effect">Effects to apply.</param>
        /// <param name="layerDepth">
        ///     The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer.
        ///     Use SpriteSortMode if you want sprites to be sorted during drawing.
        /// </param>
        /// <param name="clippingRectangle">
        /// Clips the boundaries of the text so that it's not drawn outside the clipping rectangle.
        /// </param>
        public static void DrawString(
            this SpriteBatch spriteBatch, BitmapFont font, string text, Vector2 position, Color color, float rotation,
            Vector2 origin, float scale, SpriteEffects effect, float layerDepth, RectangleF? clippingRectangle = null)
        {
            DrawString(
                spriteBatch, font, text, position, color, rotation, origin,
                new Vector2(scale, scale), effect, layerDepth, clippingRectangle);
        }

        public static void DrawString(
            this SpriteBatch spriteBatch, BitmapFont font, StringBuilder text, Vector2 position, Color color, float rotation,
            Vector2 origin, float scale, SpriteEffects effect, float layerDepth, RectangleF? clippingRectangle = null)
        {
            DrawString(
                spriteBatch, font, text, position, color, rotation, origin,
                new Vector2(scale, scale), effect, layerDepth, clippingRectangle);
        }

        /// <summary>
        ///     Adds a string to a batch of sprites for rendering using the specified font, text, position, color,
        ///     layer, and width (in pixels) where to wrap the text at.
        /// </summary>
        /// <remarks>
        ///     <see cref="BitmapFont" /> objects are loaded from the Content Manager.
        ///     See the <see cref="BitmapFont"/> class for more information.
        ///     Before any calls to DrawString you must call <see cref="SpriteBatch.Begin" />. Once all calls 
        ///     are complete, call <see cref="SpriteBatch.End" />.
        ///     Use a newline character (\n) to draw more than one line of text.
        /// </remarks>
        /// <param name="spriteBatch"></param>
        /// <param name="font">A font for displaying text.</param>
        /// <param name="text">The text message to display.</param>
        /// <param name="position">The location (in screen coordinates) to draw the text.</param>
        /// <param name="color">
        ///     The <see cref="Color" /> to tint a sprite. Use <see cref="Color.White" /> for full color with no
        ///     tinting.
        /// </param>
        /// <param name="layerDepth">
        ///     The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer.
        ///     Use SpriteSortMode if you want sprites to be sorted during drawing.
        /// </param>
        /// <param name="clippingRectangle">
        /// Clips the boundaries of the text so that it's not drawn outside the clipping rectangle.
        /// </param>
        public static void DrawString(
            this SpriteBatch spriteBatch, BitmapFont font, string text, Vector2 position,
            Color color, float layerDepth, RectangleF? clippingRectangle = null)
        {
            DrawString(
                spriteBatch, font, text, position, color, 0, Vector2.Zero,
                Vector2.One, SpriteEffects.None, layerDepth, clippingRectangle);
        }

        /// <summary>
        ///     Adds a string to a batch of sprites for rendering using the specified font, text, position, color,
        ///     and width (in pixels) where to wrap the text at. The text is drawn on layer 0f.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="font">A font for displaying text.</param>
        /// <param name="text">The text message to display.</param>
        /// <param name="position">The location (in screen coordinates) to draw the text.</param>
        /// <param name="color">
        ///     The <see cref="Color" /> to tint a sprite. Use <see cref="Color.White" /> for full color with no
        ///     tinting.
        /// </param>
        /// <param name="clippingRectangle">
        /// Clips the boundaries of the text so that it's not drawn outside the clipping rectangle.
        /// </param>
        public static void DrawString(
            this SpriteBatch spriteBatch, BitmapFont font, string text,
            Vector2 position, Color color, RectangleF? clippingRectangle = null)
        {
            DrawString(
                spriteBatch, font, text, position, color, 0, Vector2.Zero,
                Vector2.One, SpriteEffects.None, 0, clippingRectangle);
        }

        public static void DrawString(
            this SpriteBatch spriteBatch, BitmapFont font, StringBuilder text,
            Vector2 position, Color color, RectangleF? clippingRectangle = null)
        {
            DrawString(
                spriteBatch, font, text, position, color, 0, Vector2.Zero,
                Vector2.One, SpriteEffects.None, 0, clippingRectangle);
        }
    }
}