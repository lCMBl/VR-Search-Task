using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLStreamManager : MonoBehaviour {
	// TODO: have an editor class for this object which generates a static LSLEventRecorder class
	// and appropriate functions to send samples.
	private static LSLStreamManager instance;
	private Dictionary<string, liblsl.StreamOutlet> outlets = new Dictionary<string, liblsl.StreamOutlet>();
	// private static List<LSLStreamOutlet> outletInfo = new List<LSLStreamOutlet>();

	public LSLStreamDefinition[] streamDefinitions;

	[SerializeField]
	private TextAsset eventRecorderTemplate, recordStreamPartial;

	private long frameCount = 0;
	public static long FrameID {
		get { return instance.frameCount; }
	}
    void Awake() {
		outlets.Clear();
        instance = this;
		LSLEventRecorder.Init(this);

		foreach(var streamDef in streamDefinitions) {
			SetupOutlet(streamDef);
		}


        Debug.Log("LSL Manager Loaded. Make sure this script executes before default time.");
    }

	void LateUpdate() {
		// update the unity frame count ID.
		frameCount++;
	}

	// public static LSLStreamManager Get() {
	// 	if (instance == null) {
	// 		instance = new LSLStreamManager();
	// 		Debug.Log("LSL Manager Loaded");
	// 	}

	// 	return instance;
	// }

	// public static RecordSample(string streamName, )

	// public LSLStreamManager() {
	// 	Debug.Log("Creating the LSLStream Manager. Have all stream descriptions been added prior to this step?");
	// 	foreach(var outlet in outletInfo) {
	// 		SetupOutlet(outlet.name, outlet.type, outlet.channelDescriptions.ToArray(), outlet.format, outlet.sampleRate);
	// 		Debug.Log("Setup outlet " + outlet.name + " is it in dictionary? " + outlets.ContainsKey(outlet.name));
	// 	}
	// }

	// public struct ChannelDescription {
	// 	public string name;
	// 	public string info;
	// 	public Dictionary<string, string> properties;

	// 	public ChannelDescription(string name, string info) {
	// 		this.name = name;
	// 		this.info = info;
	// 		this.properties = new Dictionary<string, string>();
	// 	}
	// }

	// public struct LSLStreamOutlet {
	// 	public string name;
	// 	public string type;
	// 	public List<ChannelDescription> channelDescriptions;
	// 	public liblsl.channel_format_t format;
	// 	public float sampleRate;

	// 	public LSLStreamOutlet(string name, string type, liblsl.channel_format_t format, float sampleRate = 0f) {
	// 		this.name = name;
	// 		this.type = type;
	// 		this.format = format;
	// 		this.channelDescriptions = new List<ChannelDescription>();
	// 		this.sampleRate = sampleRate;
	// 	}
	// }

	// public static void AddOutletInfo(LSLStreamOutlet outletDesc) {
	// 	outletInfo.Add(outletDesc);
	// }

	private void SetupOutlet(LSLStreamDefinition streamDefinition){
		string streamName = streamDefinition.name.Replace(" ", "");
		Debug.Log(string.Format("Making outlet with parameters: name: {0}, type: {1}, channels:{2}, sample rate: {3}, format: {4}",
		streamName,
		streamDefinition.streamType,
		streamDefinition.channelDescriptions.Length + (streamDefinition.includeUnityFrameIDChannel ? 1 : 0),
		(double)streamDefinition.sampleRate,
		streamDefinition.channelFormat));
		var sid = System.Guid.NewGuid().ToString();

		liblsl.StreamInfo info = new liblsl.StreamInfo(
			streamName,
			streamDefinition.streamType,
			streamDefinition.channelDescriptions.Length + (streamDefinition.includeUnityFrameIDChannel ? 1 : 0),
			(double)streamDefinition.sampleRate,
			streamDefinition.channelFormat,
			sid
		);
        var chns = info.desc().append_child("channels");

		foreach (var cd in streamDefinition.channelDescriptions) {
			liblsl.XMLElement channel = info.desc().child("channels").append_child("channel");
			
			channel.append_child_value("label", cd.name.Replace(" ", "")); // IMPORTANT - CHANNEL/PROPERTY NAMES CAN'T HAVE SPACES.
			channel.append_child_value("info", cd.description);

			foreach (var prop in cd.properties) {
				// also, make sure that child names (first argument to append_child_value) (become tags) are all lowercase
				channel.append_child_value(prop.name.Replace(" ", "").ToLower(), prop.value);
			}
		}
		if (streamDefinition.includeUnityFrameIDChannel) {
			liblsl.XMLElement channel = info.desc().child("channels").append_child("channel");
			
			channel.append_child_value("label", "UnityFrameID"); // IMPORTANT - CHANNEL/PROPERTY NAMES CAN'T HAVE SPACES.
			channel.append_child_value("info", "The ID number of the Unity update frame this sample was recorded on.");
		}
		outlets[streamName] = new liblsl.StreamOutlet(info);
	}

	// public void SetupOutlet(string name, string type, ChannelDescription[] descs, liblsl.channel_format_t format, float sampleRate = 0f, string source_id = null) {
	// 	Debug.Log(string.Format("Making outlet with parameters: name: {0}, type: {1}, channels:{2}, sample rate: {3}, format: {4}", name, type, descs.Length, (double)sampleRate, format));
	// 	var sid = System.Guid.NewGuid().ToString();
	// 	if (source_id != null) {
	// 		sid = source_id;
	// 	}
	// 	liblsl.StreamInfo info = new liblsl.StreamInfo(name.ToLower(), type, descs.Length, (double)sampleRate, format, sid);
    //     var chns = info.desc().append_child("channels");
	// 	// info.desc().append_child("channels");

    //     for (int i = 0; i < descs.Length; i++) {
	// 		liblsl.XMLElement channel = info.desc().child("channels").append_child("channel");
	// 		// liblsl.XMLElement channel = chns.append_child(descs[i].name.Replace(' ', '_')); // IMPORTANT - CHANNEL/PROPERTY NAMES CAN'T HAVE SPACES.
			
	// 		channel.append_child_value("label", descs[i].name.Replace(' ', '_')); // IMPORTANT - CHANNEL/PROPERTY NAMES CAN'T HAVE SPACES.
	// 		channel.append_child_value("info", descs[i].info);

	// 		foreach (var prop in descs[i].properties) {
	// 			// also, make sure that child names (first argument to append_child_value) (become tags) are all lowercase
	// 			channel.append_child_value(prop.Key.Replace(' ', '_').ToLower(), prop.Value);
	// 		}
	// 	}
	// 	outlets[name] = new liblsl.StreamOutlet(info);
	// }

	public liblsl.StreamOutlet GetOutlet(string name) {
		return outlets[name];
	}

	
}
