﻿using ImGuiNET;
using System;
using System.Numerics;

namespace MoveFurniture {
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    unsafe class PluginUI : IDisposable {
        private Configuration configuration => Service.configuration;

        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible = false;
        public bool Visible {
            get { return visible; }
            set { visible = value; }
        }

        private bool settingsVisible = false;
        public bool SettingsVisible {
            get { return settingsVisible; }
            set { settingsVisible = value; }
        }

        public void Dispose() {

        }

        public void Draw() {
            // This is our only draw handler attached to UIBuilder, so it needs to be
            // able to draw any windows we might have open.
            // Each method checks its own visibility/state to ensure it only draws when
            // it actually makes sense.
            // There are other ways to do this, but it is generally best to keep the number of
            // draw delegates as low as possible.

            
            //DrawSettingsWindow();
        }

        public void DrawMainWindow() {
            if (!Visible) {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSizeConstraints(new Vector2(375, 330), new Vector2(float.MaxValue, float.MaxValue));
            if (ImGui.Begin("My Amazing Window", ref visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)) {
                ImGui.Text($"The random config bool is {configuration.MoveType2}");

                if (ImGui.Button("Show Settings")) {
                    SettingsVisible = true;

                }
                ImGui.End();
            }
        }

        public void DrawSettingsWindow() {
            if (!SettingsVisible) {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(232, 75), ImGuiCond.Always);
            if (ImGui.Begin("Config", ref settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)) {
                // can't ref a property, so use a local copy
                var configValue = configuration.MoveType2;
                if (ImGui.Checkbox("Random Config Bool", ref configValue)) {
                    configuration.MoveType2 = configValue;
                    // can save immediately on change, if you don't want to provide a "Save and Close" button
                    configuration.Save();
                }
            }
            ImGui.End();
        }
    }
}
