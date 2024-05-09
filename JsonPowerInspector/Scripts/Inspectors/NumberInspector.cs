using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using Godot;
using JsonPowerInspector.Template;

namespace JsonPowerInspector;

public partial class NumberInspector : BasePropertyInspector<NumberPropertyInfo>
{
    [Export] private SpinBox _contentControl;

    protected override void OnInitialize(NumberPropertyInfo propertyInfo)
    {
        _contentControl.Step = propertyInfo.NumberKind is NumberPropertyInfo.NumberType.Int ? 1 : 0.001;

        if (propertyInfo.Range.HasValue)
        {
            _contentControl.MinValue = propertyInfo.Range.Value.Lower;
            _contentControl.MaxValue = propertyInfo.Range.Value.Upper;
            _contentControl.AllowLesser = false;
            _contentControl.AllowGreater = false;
        }
        else
        {
            _contentControl.MinValue = -10000000;
            _contentControl.MaxValue = 10000000;
            _contentControl.AllowLesser = true;
            _contentControl.AllowGreater = true;
        }
    }

    public override void Bind(JsonNode node)
    {
        var value = node.AsValue();
        switch (PropertyInfo.NumberKind)
        {
            case NumberPropertyInfo.NumberType.Int:
                var contentControlValue = value.GetValue<int>();
                _contentControl.Value = contentControlValue;
                break;
            case NumberPropertyInfo.NumberType.Float:
                if (value.TryGetValue<double>(out var floatValue))
                {
                    _contentControl.Value = floatValue;
                }
                else
                {
                    contentControlValue = value.GetValue<int>();
                    _contentControl.Value = contentControlValue;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}