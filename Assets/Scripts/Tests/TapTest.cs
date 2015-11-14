using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapTest : MonoBehaviour
{
    public bool Recoding;
    public bool Playing;
    TapEvents taps = new TapEvents();

    public void Record()
    {
        StartCoroutine(record());
    }

    public void Stop()
    {
        Recoding = false;
    }

    public void Play()
    {
        StartCoroutine(play());
    }

    IEnumerator record()
    {
        taps.Clear();
        Recoding = true;
        while (Recoding)
        {
            yield return null;
            var pos = clickPosition();
            if (!pos.HasValue)
            {
                continue;
            }
            var p = pos.Value;
            var go = EventSystem.current.currentSelectedGameObject;
            taps.Add(go, (int)p.x, (int)p.y);
        }
    }

    IEnumerator play()
    {
        Playing = true;
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var events = taps.Taps;
        foreach(var e in events)
        {
            yield return new WaitForSeconds((float)(e.Seconds - sw.Elapsed.TotalSeconds));
            var ev = new PointerEventData(EventSystem.current);
            var results = new List<RaycastResult>();
            ev.position = new Vector2(e.X, e.Y);
            ev.button = PointerEventData.InputButton.Left;
            EventSystem.current.RaycastAll(ev, results);
            foreach (var result in results)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(
                    result.gameObject,
                    ev,
                    (handler, eventData) => handler.OnPointerClick((PointerEventData)eventData));
            }
        }
        Playing = false;
    }

    Vector3? clickPosition()
    {
        if(Input.GetMouseButtonDown(0))
        {
            return Input.mousePosition;
        }
        return null;
    }

    class TapEvents
    {
        public List<TapEvent> Taps = new List<TapEvent>();
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        public void Add(GameObject go, int x, int y)
        {
            if(Taps.Count == 0)
            {
                sw.Reset();
                sw.Start();
            }

            var names = new List<string>();
            var t = go.transform;
            while(t != null)
            {
                names.Add(t.name);
                t = t.parent;
            }
            var name = string.Join("/", names.ToArray());
            var tap = new TapEvent(name, x, y, sw.Elapsed.TotalSeconds);
            Taps.Add(tap);
        }

        public void Clear()
        {
            sw.Stop();
            Taps.Clear();
        }
    }

    class TapEvent
    {
        public readonly string Name;
        public readonly int X;
        public readonly int Y;
        public readonly double Seconds;

        public TapEvent(string name, int x, int y, double seconds)
        {
            Name = name;
            X = x;
            Y = y;
            Seconds = seconds;
        }
    }
}
