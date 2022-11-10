using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

// Special signal used to pause the timeline precisely
public class PauseSignalEmitter : SignalEmitter { }

// Attach this component to the PlayableDirector in order to correctly setup the pauses
[RequireComponent(typeof(PlayableDirector))]
public class TimelinePausesManager : MonoBehaviour
{
    private PlayableDirector _director;
    private List<PauseSignalEmitter> _markers;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
        _markers = GetAllMarkers<PauseSignalEmitter>(_director);

        if (_markers.Count > 0)
            SetupPauses();
    }

    private void SetupPauses()
    {
        // Update timeline duration now and on any play/pause events
        //UpdateTimelineDuration();
        _director.played += _ => UpdateTimelineDuration();
        _director.paused += _ => UpdateTimelineDuration();

        // Make sure that the signal receiver exists
        var signalReceiver = GetComponent<SignalReceiver>();
        if (signalReceiver == null)
            signalReceiver = gameObject.AddComponent<SignalReceiver>();

        // Pause the timeline automatically on each pause marker
        foreach (var marker in _markers)
        {
            var reaction = signalReceiver.GetReaction(marker.asset);
            if (reaction == null)
            {
                reaction = new UnityEvent();
                signalReceiver.AddReaction(marker.asset, reaction);
            }
            reaction.AddListener(_director.Pause);
        }
    }

    private void UpdateTimelineDuration()
    {
        // Find the next pause marker that will be hit
        var nextMarker = _markers
            .Where(m => m.time > _director.time)
            .FirstOrDefault();

        // Force the timeline duration to end exactly on that marker
        // (or to the original duration if there are no pause markers ahead)
        var nextMarkerTime = nextMarker != null ? nextMarker.time : _director.duration;
        _director.playableGraph.GetRootPlayable(0).SetDuration(nextMarkerTime);
    }

    /// Find all markers of a specified type
    private static List<T> GetAllMarkers<T>(PlayableDirector director) where T : IMarker
    {
        List<T> markers = new List<T>();

        var timeline = director.playableAsset as TimelineAsset;
        if (timeline == null)
            return markers;

        if (timeline.markerTrack == null)
            return markers;

        foreach (var marker in timeline.markerTrack.GetMarkers())
            if (marker is T tMarker)
                markers.Add(tMarker);

        return markers;
    }
}