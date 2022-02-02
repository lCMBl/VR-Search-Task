using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LSL;

[CreateAssetMenu(fileName = "Stream Definition", menuName = "LSL/LSLStreamDefinition", order = 1)]
public class LSLStreamDefinition : ScriptableObject
{
    // goal of this class is to store the configuration information for a single LSL stream.
    // it is mostly composed of multiple channel definitions, but it also must implement a 
    // ToSample function, which returns an array of the type used by the stream
    public string streamType;
    public liblsl.channel_format_t channelFormat;
    public float sampleRate = 0f;
    public bool includeUnityFrameIDChannel = true;
    public SampleInputArgument[] inputArguments;
    public ChannelDescription[] channelDescriptions;

    public string SampleType {
        get {
            string st = "";
            switch(channelFormat) {
                case liblsl.channel_format_t.cf_double64:
                    st = "double";
                    break;
                case liblsl.channel_format_t.cf_float32:
                    st = "float";
                    break;
                case liblsl.channel_format_t.cf_int16:
                    st = "short";
                    break;
                case liblsl.channel_format_t.cf_int32:
                    st = "int";
                    break;
                case liblsl.channel_format_t.cf_int64:
                    st = "long";
                    break;
                case liblsl.channel_format_t.cf_int8:
                    st = "sbyte";
                    break;
                case liblsl.channel_format_t.cf_string:
                    st = "string";
                    break;
                default:
                    st = "INVALID_CHANNEL_TYPE";
                    break; 
            }
            return st; 
        }
    }

    [Serializable]
    public struct SampleInputArgument {
        public string variableName, type;
    }

    [Serializable]
    public struct ChannelProperty {
        public string name, value;
    }

    [Serializable]
    public struct ChannelDescription {
        public string name, description, argumentPath;
        public ChannelProperty[] properties;

        public ChannelDescription(string name, string description, string argumentPath, ChannelProperty[] properties) {
            this.name = name;
            this.description = description;
            this.argumentPath = argumentPath;
            this.properties = properties;
        }
    }

    // worst case, we can make a push sample function that checks for type and length of the given sample.
    // in order to make this worth while, we need to be able to map arbitrary inputs onto a list of outputs. 
    // Don't worry about executing functions on inputs, just specify the end result of the function as an input.

}
