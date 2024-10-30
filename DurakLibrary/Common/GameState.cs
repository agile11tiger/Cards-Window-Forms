using DurakLibrary.Cards;
using DurakLibrary.HostServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DurakLibrary.Common
{
    public delegate void StateChangedEvent(GameState sender, StateParameter parameter);

    public class GameState
    {
        public event EventHandler<StateParameter> OnStateChanged;
        public bool SilentSets { get; set; }

        public GameState()
        {
            parameters = new Dictionary<string, StateParameter>();
            changedEvents = new Dictionary<string, StateChangedEvent>();
            stateEqualsEvents = new Dictionary<Tuple<string, object>, StateChangedEvent>();
        }

        public void Clear()
        {
            parameters.Clear();
            changedEvents.Clear();
            stateEqualsEvents.Clear();
        }

        public void AddStateChangedEvent(string name, StateChangedEvent eventListener)
        {
            if (!changedEvents.ContainsKey(name))
                changedEvents.Add(name, eventListener);
            else
                changedEvents[name] += eventListener;
        }

        public void RemoveStateChangedEvent(string name, StateChangedEvent eventListener)
        {
            if (!changedEvents.ContainsKey(name))
                changedEvents.Add(name, eventListener);
            else
                changedEvents[name] -= eventListener;
        }

        public void AddStateEqualsEvent(string name, object value, StateChangedEvent eventListener)
        {
            var key = new Tuple<string, object>(name, value);

            if (!stateEqualsEvents.ContainsKey(key))
                stateEqualsEvents.Add(key, eventListener);
            else
                stateEqualsEvents[key] += eventListener;
        }

        public void RemoveStateEqualsEvent(string name, object value, StateChangedEvent eventListener)
        {
            var key = new Tuple<string, object>(name, value);

            if (!stateEqualsEvents.ContainsKey(key))
                stateEqualsEvents.Add(key, eventListener);
            else
                stateEqualsEvents[key] -= eventListener;
        }

        public void AddStateChangedEvent(string name, int index, StateChangedEvent eventListener)
        {
            name = string.Format(ARRAY_FORMAT, name, index);

            if (!changedEvents.ContainsKey(name))
                changedEvents.Add(name, eventListener);
            else
                changedEvents[name] += eventListener;
        }

        public void RemoveStateChangedEvent(string name, int index, StateChangedEvent eventListener)
        {
            name = string.Format(ARRAY_FORMAT, name, index);

            if (!changedEvents.ContainsKey(name))
                changedEvents.Add(name, eventListener);
            else
                changedEvents[name] -= eventListener;
        }

        public void AddStateEqualsEvent(string name, int index, object value, StateChangedEvent eventListener)
        {
            name = string.Format(ARRAY_FORMAT, name, index);
            var key = new Tuple<string, object>(name, value);

            if (!stateEqualsEvents.ContainsKey(key))
                stateEqualsEvents.Add(key, eventListener);
            else
                stateEqualsEvents[key] += eventListener;
        }

        public void RemoveStateEqualsEvent(string name, int index, object value, StateChangedEvent eventListener)
        {
            name = string.Format(ARRAY_FORMAT, name, index);
            var key = new Tuple<string, object>(name, value);

            if (!stateEqualsEvents.ContainsKey(key))
                stateEqualsEvents.Add(key, eventListener);
            else
                stateEqualsEvents[key] -= eventListener;
        }

        public StateParameter GetParameter<T>(string name, bool serverSide = false)
        {
            if (!parameters.ContainsKey(name))
                parameters.Add(name, StateParameter.Construct<T>(name, (T)Activator.CreateInstance(typeof(T)), serverSide));

            return parameters[name];
        }

        public void UpdateParam(StateParameter parameter)
        {
            if (parameters.ContainsKey(parameter.Name))
                InternalSet(parameter.Name, parameter.RawValue, !parameter.IsSynced);
            else
                parameters.Add(parameter.Name, parameter);
        }

        public void Set<T>(string name, T value, bool serverSide = false)
        {
            if (string.IsNullOrWhiteSpace(name) || name[0] == '@')
                throw new ArgumentException("Invalid name, cannot be empty or start with @");

            if (!StateParameter.SupportedTypes.ContainsKey(typeof(T)))
                throw new ArgumentException("Type " + typeof(T) + " is not a supported type");

            InternalSet(name, value, serverSide);
        }

        public void Set<T>(string name, int index, T value, bool serverSide = false)
        {
            if (!StateParameter.SupportedTypes.ContainsKey(typeof(T)))
                throw new ArgumentException("Type " + typeof(T) + " is not a supported type");

            InternalSet(string.Format(ARRAY_FORMAT, name, index), value, serverSide);
        }

        public byte GetValueByte(string name) => GetValueInternal<byte>(name);
        public byte GetValueByte(string name, int index) => GetValueInternal<byte>(string.Format(ARRAY_FORMAT, name, index));
        public char GetValueChar(string name) => GetValueInternal<char>(name);
        public char GetValueChar(string name, int index) => GetValueInternal<char>(string.Format(ARRAY_FORMAT, name, index));
        public short GetValueShort(string name) => GetValueInternal<short>(name);
        public short GetValueShort(string name, int index) => GetValueInternal<short>(string.Format(ARRAY_FORMAT, name, index));
        public int GetValueInt(string name) => GetValueInternal<int>(name);
        public int GetValueInt(string name, int index) => GetValueInternal<int>(string.Format(ARRAY_FORMAT, name, index));
        public bool GetValueBool(string name) => GetValueInternal<bool>(name);
        public bool GetValueBool(string name, int index) => GetValueInternal<bool>(string.Format(ARRAY_FORMAT, name, index));
        public CardValue GetValueCardValue(string name) => GetValueInternal<CardValue>(name);
        public CardValue GetValueCardValue(string name, int index) => GetValueInternal<CardValue>(string.Format(ARRAY_FORMAT, name, index));
        public CardSuit GetValueCardSuit(string name) => GetValueInternal<CardSuit>(name);
        public CardSuit GetValueCardSuit(string name, int index) => GetValueInternal<CardSuit>(string.Format(ARRAY_FORMAT, name, index));
        public string GetValueString(string name) => GetValueInternal<string>(name);
        public string GetValueString(string name, int index) => GetValueInternal<string>(string.Format(ARRAY_FORMAT, name, index));
        public Card GetValueCard(string name) => GetValueInternal<Card>(name);
        public Card GetValueCard(string name, int index) => GetValueInternal<Card>(string.Format(ARRAY_FORMAT, name, index));
        public CardCollection GetValueCardCollection(string name) => GetValueInternal<CardCollection>(name);
        public List<int> GetValueListInt(string name) => GetValueInternal<List<int>>(name);
        public Dictionary<int, bool> GetValueDictIntBool(string name) => GetValueInternal<Dictionary<int, bool>>(name);
        public StateParameter[] GetParameterCollection() => parameters.Values.ToArray();

        public bool Equals(string name, object value)
        {
            if (parameters.ContainsKey(name))
                if (parameters[name].RawValue == null)
                    return value == null;
                else
                    return parameters[name].RawValue.Equals(value);
            else
                return value == null;
        }

        public void Encode(BinaryWriter writer)
        {
            StateParameter[] toTransfer = parameters.Values.Where(x => x.IsSynced).ToArray();
            writer.Write(toTransfer.Length);

            for (int index = 0; index < toTransfer.Length; index++)
                toTransfer[index].Encode(writer);
        }

        public void Decode(BinaryReader reader)
        {
            var numParams = reader.ReadInt32();

            for (int index = 0; index < numParams; index++)
                StateParameter.Decode(reader, this);
        }

        public void UpdateParameters()
        {
            StateParameter[] toTransfer = parameters.Values.Where(x => x.IsSynced).ToArray();

            for (int index = 0; index < toTransfer.Length; index++)
                InvokeUpdated(toTransfer[index]);
        }

        internal void InvokeUpdated(StateParameter stateParameter)
        {
            if (stateParameter != null)
            {
                if (!SilentSets && OnStateChanged != null)
                    OnStateChanged(this, stateParameter);

                if (changedEvents.ContainsKey(stateParameter.Name))
                    changedEvents[stateParameter.Name](this, stateParameter);
                else
                {
                    string name = changedEvents.Keys.FirstOrDefault(X => stateParameter.Name.Substring(1, stateParameter.Name.Length - 1) == X);
                    if (name != null)
                        changedEvents[name](this, stateParameter);
                }

                Tuple<string, object> key = stateEqualsEvents.Keys.FirstOrDefault(X => X.Item1 == stateParameter.Name);

                if (key != null && stateParameter.RawValue.Equals(key.Item2))
                    stateEqualsEvents[key](this, stateParameter);
            }
        }

        private const string ARRAY_FORMAT = "@{0}[{1}]";
        private Dictionary<string, StateParameter> parameters;
        private Dictionary<string, StateChangedEvent> changedEvents;
        private Dictionary<Tuple<string, object>, StateChangedEvent> stateEqualsEvents;

        private void InternalSet<T>(string name, T value, bool serverSide)
        {
            if (!parameters.ContainsKey(name))
                parameters.Add(name, StateParameter.Construct(name, value, !serverSide));
            else
                parameters[name].SetValueInternal(value);

            InvokeUpdated(parameters[name]);
        }

        private T GetValueInternal<T>(string name, bool serverSide = false)
        {
            if (parameters.ContainsKey(name))
            {
                object value = parameters[name].RawValue;

                if (value == null)
                    return default(T);
                else if (typeof(T).IsAssignableFrom(value.GetType()))
                    return (T)value;
                else if (Utils.CanChangeType(value, typeof(T)))
                    return (T)Convert.ChangeType(value, typeof(T));
                else
                    throw new InvalidCastException(string.Format("Cannot cast {0} to {1}", value.GetType().Name, typeof(T).Name));
            }
            else
            {
                parameters.Add(name, StateParameter.Construct(name, default(T), !serverSide));
                return parameters[name].GetValueInternal<T>();
            }
        }
    }
}
