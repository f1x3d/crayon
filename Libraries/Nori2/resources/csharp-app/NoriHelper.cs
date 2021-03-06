using Interpreter.Structs;
using System;
using System.Collections.Generic;

namespace Interpreter.Libraries.Nori2
{
    public static class NoriHelper
    {
        public static void AddChildToParent(object child, object parent)
        {
            ((System.Windows.Forms.Panel)parent).Controls.Add((System.Windows.Forms.Control)child);
        }

        public static void CloseWindow(object window)
        {
            throw new NotImplementedException();
        }

        public static void EnsureParentLinkOrder(object parent, object[] children)
        {
            System.Windows.Forms.Control.ControlCollection actualCollection = ((System.Windows.Forms.Control)parent).Controls;
            System.Windows.Forms.Control[] actualOrder = GetControls(actualCollection);

            int perfectMatchUntil = 0;
            if (actualOrder.Length == children.Length)
            {
                for (int i = 0; i < children.Length; ++i)
                {
                    if (children[i] == actualOrder[i])
                    {
                        perfectMatchUntil = i + 1;
                    }
                }
                if (perfectMatchUntil == children.Length)
                {
                    return;
                }
            }
            throw new NotImplementedException();
        }

        public static object InstantiateElement(int type, ElementProperties properties)
        {
            int left = properties.render_left;
            int top = properties.render_top;
            int width = properties.render_width;
            int height = properties.render_height;

            switch (type)
            {
                case 1: // Rectangle
                    Rectangle rect = new Rectangle();
                    rect.SetColor(
                        properties.bg_red,
                        properties.bg_green,
                        properties.bg_blue,
                        properties.bg_alpha);
                    return rect;

                case 2: // Canvas
                    Canvas canvas = new Canvas();
                    canvas.SetPosition(left, top, width, height);
                    return canvas;

                case 3: // ScrollPanel
                    ScrollPanel scrollPanel = new ScrollPanel();
                    scrollPanel.SetPosition(left, top, width, height);
                    return scrollPanel;

                case 4: // Button
                    Button button = new Button();
                    button.SetPosition(left, top, width, height);
                    button.Text = properties.misc_string_0;
                    return button;

                default:
                    throw new NotImplementedException();
            }
        }

        public static object InstantiateWindow(WindowProperties properties)
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.Text = properties.title;
            form.Size = new System.Drawing.Size(properties.width, properties.height);
            return form;
        }

        public static void InvalidateElementProperty(int type, object element, int key, object value)
        {
            throw new NotImplementedException();
        }

        public static void InvalidateWindowProperty(object window, int key, object value)
        {
            throw new NotImplementedException();
        }

        public static void ShowWindow(object window, object[] ignored, object rootElement)
        {
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)window;
            form.Controls.Add((System.Windows.Forms.Panel)rootElement);
            form.ShowDialog();
        }

        public static System.Windows.Forms.Control[] GetControls(System.Windows.Forms.Control.ControlCollection cc)
        {
            List<System.Windows.Forms.Control> output = new List<System.Windows.Forms.Control>();
            foreach (System.Windows.Forms.Control c in cc)
            {
                output.Add(c);
            }
            return output.ToArray();
        }

        public static void UpdateLayout(object element, int typeId, int x, int y, int width, int height)
        {
            switch (typeId)
            {
                case 1:
                    Rectangle r = (Rectangle)element;
                    r.SetPosition(x, y, width, height);
                    break;

                case 3:
                    ScrollPanel sp = (ScrollPanel)element;
                    sp.SetPosition(x, y, width, height);
                    break;

                case 4:
                    Button btn = (Button)element;
                    btn.SetPosition(x, y, width, height);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
