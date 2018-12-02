﻿using MonoGame.Extended.Collections;
using System;
using System.Text;

namespace MonoGame.Extended.BitmapFonts
{
    public static class CharIteratorPool
    {
        public const int LargeBuilderThreshold = 360;
        public const int CharBufferSize = 1024 * 8;

        private static Bag<StringBuilder> _smallBuilders;
        private static Bag<StringBuilder> _largeBuilders;

        [ThreadStatic]
        private static WeakReference<char[]> _charBuffer;

        private static Bag<StringCharIterator> _stringIterators;
        private static Bag<StringBuilderCharIterator> _builderIterators;

        static CharIteratorPool()
        {
            _smallBuilders = new Bag<StringBuilder>();
            _largeBuilders = new Bag<StringBuilder>();

            _stringIterators = new Bag<StringCharIterator>();
            _builderIterators = new Bag<StringBuilderCharIterator>();
        }

        public static ICharIterator Rent(string value, int offset, int count)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            lock (_stringIterators)
            {
                if (_stringIterators.TryTake(out var result))
                {
                    result.Set(value, offset, count);
                    return result;
                }
            }
            return new StringCharIterator(value, offset, count);
        }

        public static ICharIterator Rent(StringBuilder value, int offset, int count)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            StringBuilder immutableBuilder = count < LargeBuilderThreshold ?
                RentSmallBuilder() : RentLargeBuilder();

            char[] buffer = GetCharBuffer();
            value.CopyTo(offset, immutableBuilder, buffer, count);

            lock (_builderIterators)
            {
                if (_builderIterators.TryTake(out var result))
                {
                    result.Set(immutableBuilder, offset, count);
                    return result;
                }
            }
            return new StringBuilderCharIterator(immutableBuilder, offset, count);
        }

        public static void Return(ICharIterator iterator)
        {
            if (iterator is StringCharIterator stringIterator)
            {
                lock (_stringIterators)
                {
                    if (stringIterator._isInUse)
                    {
                        stringIterator.Set(null, 0, 0);
                        _stringIterators.Add(stringIterator);
                    }
                }
            }
            else if (iterator is StringBuilderCharIterator builderIterator)
            {
                lock (_builderIterators)
                {
                    if (builderIterator._isInUse)
                    {
                        ReturnBuilder(builderIterator._builder);
                        builderIterator.Set(null, 0, 0);
                        _builderIterators.Add(builderIterator);
                    }
                }
            }
            else
                throw new ArgumentException("The iterator was not rented from this pool.");
        }

        public static StringBuilder RentSmallBuilder()
        {
            lock (_smallBuilders)
            {
                if (_smallBuilders.TryTake(out var result))
                {
                    result.Clear();
                    return result;
                }
            }
            return new StringBuilder();
        }

        public static StringBuilder RentLargeBuilder()
        {
            lock (_largeBuilders)
            {
                if (_largeBuilders.TryTake(out var result))
                {
                    result.Clear();
                    return result;
                }
            }
            return new StringBuilder(LargeBuilderThreshold);
        }

        public static void ReturnBuilder(StringBuilder builder)
        {
            if (builder.Capacity < LargeBuilderThreshold)
            {
                lock (_smallBuilders)
                {
                    if (_smallBuilders.Count < 128)
                        _smallBuilders.Add(builder);
                }
            }
            else
            {
                lock (_largeBuilders)
                {
                    if (_largeBuilders.Count < 64)
                        _largeBuilders.Add(builder);
                }
            }
        }

        private static char[] GetCharBuffer()
        {
            // _charBuffer is thread-static, no locks needed here

            char[] buffer;
            if (_charBuffer == null)
            {
                buffer = new char[CharBufferSize];
                _charBuffer = new WeakReference<char[]>(buffer);
            }
            else if (!_charBuffer.TryGetTarget(out buffer))
            {
                buffer = new char[CharBufferSize];
                _charBuffer.SetTarget(buffer);
            }
            return buffer;
        }
    }
}
