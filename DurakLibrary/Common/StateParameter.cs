using DurakLibrary.Cards;
using DurakLibrary.HostServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DurakLibrary.Common
{
    public class StateParameter : EventArgs
    {
        public static StateParameter CreateEmpty(bool sync = false) => new StateParameter() { IsSynced = sync };
        public static readonly Dictionary<System.Type, Type> SupportedTypes = new Dictionary<System.Type, Type>()
        {
            { typeof(byte), Type.Byte },
            { typeof(char), Type.Char },
            { typeof(short), Type.Short },
            { typeof(int), Type.Int },
            { typeof(bool), Type.Bool },
            { typeof(CardSuit), Type.CardSuit },
            { typeof(CardValue), Type.CardValue },
            { typeof(string), Type.String },
            { typeof(Card), Type.Card },
            { typeof(CardCollection), Type.CardCollection },
            { typeof(List<int>), Type.ListInt },
            { typeof(Dictionary<int, bool>), Type.DictIntBool }
        };

        public string Name { get => name; }
        public object RawValue { get => value; }
        public Type ParameterType { get => type; }
        public bool IsSynced { get; set; }

        public static StateParameter Construct<T>(string name, T value, bool syncronize)
        {
            var result = new StateParameter();
            result.name = name;
            result.type = SupportedTypes[typeof(T)];
            result.value = value;
            result.IsSynced = syncronize;
            return result;
        }

        internal T GetValueInternal<T>()
        {
            if (value == null)
                return default(T);
            else if (value is T) 
                return (T)value;
            else 
                throw new InvalidCastException(string.Format("Cannot cast {0} to {1}", value.GetType().Name, typeof(T).Name));
        }

        public byte GetValueByte() => GetValueInternal<byte>();
        public int GetValueChar() => GetValueInternal<char>();
        public int GetValueShort() => GetValueInternal<short>();
        public int GetValueInt() => GetValueInternal<int>();
        public bool GetValueBool() => GetValueInternal<bool>();
        public CardSuit GetValueCardSuit() => GetValueInternal<CardSuit>();
        public CardValue GetValueCardValue() => GetValueInternal<CardValue>();
        public string GetValueString() => GetValueInternal<string>();
        public Card GetValueCard() => GetValueInternal<Card>();
        public CardCollection GetValueCardCollection() => GetValueInternal<CardCollection>();
        public List<int> GetValueListInt() => GetValueInternal<List<int>>();
        public Dictionary<int, bool> GetValueDictIntBool() => GetValueInternal<Dictionary<int, bool>>();

        internal void SetValueInternal<T>(T value)
        {
            var type = typeof(T);
            var valueType = SupportedTypes.FirstOrDefault(X => X.Value == ParameterType).Key;

            if (SupportedTypes.ContainsKey(type))
            {
                if (type == valueType)
                    this.value = value;
                else if (Utils.CanChangeType(value, valueType))
                    this.value = Convert.ChangeType(value, valueType);
            }
            else
                throw new InvalidCastException("Type " + type + " is not supported");
        }

        public void Encode(BinaryWriter writer)
        {
            if (IsSynced)
            {
                writer.Write(Name);
                writer.Write((byte)ParameterType);

                switch (ParameterType)
                {
                    case Type.Byte:
                        writer.Write((byte)value);
                        break;
                    case Type.Char:
                        writer.Write((char)value);
                        break;
                    case Type.Short:
                        writer.Write((short)value);
                        break;
                    case Type.Int:
                        writer.Write((int)value);
                        break;
                    case Type.Bool:
                        writer.Write((bool)value);
                        break;
                    case Type.CardSuit:
                        writer.Write((byte)(CardSuit)value);
                        break;
                    case Type.CardValue:
                        writer.Write((byte)(CardSuit)value);
                        break;
                    case Type.String:
                        writer.Write((string)value);
                        break;
                    case Type.Card:
                        writer.Write(value != null);

                        if (value != null)
                        {
                            var card = value as Card;
                            writer.Write((byte)card.Value);
                            writer.Write((byte)card.Suit);
                        }
                        break;
                    case Type.CardCollection:
                        var cardCollection = value as CardCollection;
                        writer.Write(cardCollection.Count);

                        foreach (var card in cardCollection)
                        {
                            writer.Write(card != null);

                            if (card != null)
                            {
                                writer.Write((byte)card.Value);
                                writer.Write((byte)card.Suit);
                            }
                        }
                        break;
                    case Type.ListInt:
                        var list = value as List<int>;
                        writer.Write(list.Count);

                        foreach (int? number in list)
                        {
                            writer.Write(number != null);

                            if (number != null)
                                writer.Write((int)number);

                        }
                        break;
                    case Type.DictIntBool:
                        var dict = value as Dictionary<int, bool>;
                        writer.Write(dict.Count);

                        foreach (var pair in dict)
                        {
                            writer.Write(pair.Key);
                            writer.Write(pair.Value);
                        }
                        break;
                }
            }
        }

        public void Decode(BinaryReader reader)
        {
            name = reader.ReadString();
            type = (Type)reader.ReadByte();
            DecodeInternal(reader);
        }

        public static StateParameter Decode(BinaryReader reader, GameState state)
        {
            var name = reader.ReadString();
            var type = (Type)reader.ReadByte();
            StateParameter result = null;

            switch (type)
            {
                case Type.Byte:
                    result = state.GetParameter<byte>(name);
                    break;
                case Type.Char:
                    result = state.GetParameter<char>(name);
                    break;
                case Type.Short:
                    result = state.GetParameter<short>(name);
                    break;
                case Type.Int:
                    result = state.GetParameter<int>(name);
                    break;
                case Type.Bool:
                    result = state.GetParameter<bool>(name);
                    break;
                case Type.CardSuit:
                    result = state.GetParameter<CardSuit>(name);
                    break;
                case Type.CardValue:
                    result = state.GetParameter<CardValue>(name);
                    break;
                case Type.String:
                    result = state.GetParameter<String>(name);
                    break;
                case Type.Card:
                    result = state.GetParameter<Card>(name);
                    break;
                case Type.CardCollection:
                    result = state.GetParameter<CardCollection>(name);
                    break;
                case Type.ListInt:
                    result = state.GetParameter<List<int>>(name);
                    break;
                case Type.DictIntBool:
                    result = state.GetParameter<Dictionary<int, bool>>(name);
                    break;
            }

            result.DecodeInternal(reader);             
            state.InvokeUpdated(result);
            return result;
        }

        public override string ToString() => value?.ToString();

        private string name;
        private object value;
        private Type type;

        private void DecodeInternal(BinaryReader clientReader)
        {
            switch (ParameterType)
            {
                case Type.Byte:
                    value = clientReader.ReadByte();
                    break;
                case Type.Char:
                    value = (char)clientReader.ReadByte();
                    break;
                case Type.Short:
                    value = clientReader.ReadInt16();
                    break;
                case Type.Int:
                    value = clientReader.ReadInt32();
                    break;
                case Type.Bool:
                    value = clientReader.ReadBoolean();
                    break;
                case Type.CardSuit:
                    value = (CardSuit)clientReader.ReadByte();
                    break;
                case Type.CardValue:
                    value = (CardValue)clientReader.ReadByte();
                    break;
                case Type.String:
                    value = clientReader.ReadString();
                    break;
                case Type.Card:
                    if (clientReader.ReadBoolean())
                        value = new Card((CardValue)clientReader.ReadByte(), (CardSuit)clientReader.ReadByte())
                        { FaceUp = true };
                    else
                        value = null;
                    break;
                case Type.CardCollection:
                    var resultCollection = new CardCollection();
                    var numCards = clientReader.ReadInt32();

                    for (int index = 0; index < numCards; index++)
                    {
                        var hasValue = clientReader.ReadBoolean();

                        if (hasValue)
                            resultCollection.Add(new Card((CardValue)clientReader.ReadByte(), (CardSuit)clientReader.ReadByte()) 
                            { FaceUp = true });
                    }

                    value = resultCollection;
                    break;
                case Type.ListInt:
                    var list = new List<int>();
                    var number = clientReader.ReadInt32();

                    for (int index = 0; index < number; index++)
                    {
                        var hasValue = clientReader.ReadBoolean();

                        if (hasValue)
                            list.Add(clientReader.ReadInt32());
                    }

                    value = list;
                    break;
                case Type.DictIntBool:
                    var dict = new Dictionary<int, bool>();
                    var amount = clientReader.ReadInt32();

                    for (int index = 0; index < amount; index++)
                        dict.Add(clientReader.ReadInt32(), clientReader.ReadBoolean());

                    value = dict;
                    break;
            }
        }

        public enum Type
        {
            Byte,
            Char,
            Short,
            Int,
            Bool,
            CardSuit,
            CardValue,
            String,
            Card,
            CardCollection,
            ListInt,
            DictIntBool
        }
    }
}
