﻿using System;
using Blazor.Diagrams.Core.Models.Base;
using Blazor.Diagrams.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Diagrams.Components.Renderers;

public class LinkRenderer : ComponentBase, IDisposable
{
    private bool _shouldRender = true;

    [CascadingParameter] public BlazorDiagram BlazorDiagram { get; set; }

    [Parameter] public BaseLinkModel Link { get; set; }

    public void Dispose()
    {
        Link.Changed -= OnLinkChanged;
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Link.Changed += OnLinkChanged;
    }

    protected override bool ShouldRender()
    {
        return _shouldRender;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var componentType = BlazorDiagram.GetComponent(Link) ?? typeof(LinkWidget);

        builder.OpenElement(0, "g");
        builder.AddAttribute(1, "class", "link");
        builder.AddAttribute(2, "data-link-id", Link.Id);
        builder.AddAttribute(3, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerDown));
        builder.AddEventStopPropagationAttribute(4, "onpointerdown", true);
        builder.AddAttribute(5, "onpointerup", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerUp));
        builder.AddEventStopPropagationAttribute(6, "onpointerup", true);
        builder.AddAttribute(7, "onmouseenter", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseEnter));
        builder.AddAttribute(8, "onmouseleave", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseLeave));
        builder.OpenComponent(9, componentType);
        builder.AddAttribute(10, "Link", Link);
        builder.CloseComponent();
        builder.CloseElement();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        _shouldRender = false;
    }

    private void OnLinkChanged(Model _)
    {
        _shouldRender = true;
        InvokeAsync(StateHasChanged);
    }

    private void OnPointerDown(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerDown(Link, e.ToCore());
    }

    private void OnPointerUp(PointerEventArgs e)
    {
        BlazorDiagram.TriggerPointerUp(Link, e.ToCore());
    }

    private void OnMouseEnter(MouseEventArgs e)
    {
        BlazorDiagram.TriggerPointerEnter(Link, e.ToCore());
    }

    private void OnMouseLeave(MouseEventArgs e)
    {
        BlazorDiagram.TriggerPointerLeave(Link, e.ToCore());
    }
}