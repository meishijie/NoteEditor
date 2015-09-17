﻿using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class WaveformRenderer : MonoBehaviour
{
    [SerializeField]
    Color color;

    void Awake()
    {
        var model = NotesEditorModel.Instance;
        var waveData = new float[500000];
        var skipSamples = 50;
        var lines = Enumerable.Range(0, waveData.Length / skipSamples)
            .Select(_ => new Line(Vector3.zero, Vector3.zero, color))
            .ToArray();


        this.LateUpdateAsObservable()
            .Where(_ => model.WaveformDisplayEnabled.Value)
            .SkipWhile(_ => model.Audio.clip == null)
            .Subscribe(_ =>
            {
                var timeSamples = Mathf.Min(model.Audio.timeSamples, model.Audio.clip.samples - 1);
                model.Audio.clip.GetData(waveData, timeSamples);

                var x = (model.CanvasWidth.Value / model.Audio.clip.samples) / 2f;
                var offsetX = model.CanvasOffsetX.Value / model.CanvasScaleFactor.Value;
                var offsetY = 200;

                for (int li = 0, wi = skipSamples / 2, l = waveData.Length; wi < l; li++, wi += skipSamples)
                {
                    lines[li].start.x = lines[li].end.x = wi * x + offsetX;
                    lines[li].end.y = waveData[wi] * 45 - offsetY;
                    lines[li].start.y = waveData[wi - skipSamples / 2] * 45 - offsetY;
                    lines[li].start = model.CanvasToScreenPosition(lines[li].start);
                    lines[li].end = model.CanvasToScreenPosition(lines[li].end);
                }

                GLLineRenderer.Render("waveform", lines);
            });
    }
}
