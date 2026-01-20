using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.OpenGL.Extensions.ImGui;
using PhysicProject.Models;

namespace PhysicProject
{
    public class ImGuiViewModel : IDisposable
    {
        private IWindow? _window;
        private GL? _gl;
        private ImGuiController? _controller;
        private bool _showDemoWindow = false;
        private bool _showCustomWindow = true;
        private bool _showGraphWindow = true;
        private bool _showAboutWindow = false;
        private bool _requestAboutWindow = false;
        private bool _checkboxValue = false;
        private int _comboSelection = 0;
        private readonly string[] _comboItems = { "Opciya 1", "Opciya 2", "Opciya 3" };
        private Vector3 _color = new Vector3(1.0f, 0.0f, 0.0f);
        private DataProcess _dataProcess = new DataProcess();
        private readonly Params _params = new Params();

        // Данные для графика
        

        public void Run()
        {
            // Создание окна
            var options = WindowOptions.Default;
            options.Size = new Silk.NET.Maths.Vector2D<int>(1280, 720);
            options.Title = "Physic model";
            options.VSync = true;

            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            _window.Closing += OnClosing;

            _window.Run();
        }

        private void OnLoad()
        {
            _gl = _window!.CreateOpenGL();
            IInputContext input = _window!.CreateInput();

            _controller = new ImGuiController(_gl, _window, input);


        }

        private void OnUpdate(double deltaTime)
        {
            _controller!.Update((float)deltaTime);

            // Обновляем данные графика
            
        }

        private void OnRender(double deltaTime)
        {
            _gl!.ClearColor(0.2f, 0.2f, 0.25f, 1.0f);
            _gl.Clear(ClearBufferMask.ColorBufferBit);

            _controller!.MakeCurrent();
            
            Params param1 = _params;

            // Главное меню
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("open"))
                    {
                        Console.WriteLine("Menu: Otkryt'");
                    }
                    if (ImGui.MenuItem("save"))
                    {
                        Console.WriteLine("Menu: Sohranit'");
                    }
                    ImGui.Separator();
                    if (ImGui.MenuItem("exit"))
                    {
                        _window!.Close();
                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Okna"))
                {
                    ImGui.Checkbox("Demo okno", ref _showDemoWindow);
                    ImGui.Checkbox("Kastomnoe okno", ref _showCustomWindow);
                    ImGui.Checkbox("Grafik", ref _showGraphWindow);
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("info"))
                {
                    if (ImGui.MenuItem("Informaciya"))
                    {
                        _requestAboutWindow = true;
                    }
                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }

            // Демо окно ImGui
            if (_showDemoWindow)
            {
                ImGui.ShowDemoWindow(ref _showDemoWindow);
            }

            // Кастомное окно с элементами управления
            if (_showCustomWindow)
            {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(0, 20));
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 600));
                if (ImGui.Begin("Moe okno", ImGuiWindowFlags.NoMove |
                                            ImGuiWindowFlags.NoResize |
                                            ImGuiWindowFlags.NoCollapse)) 
                                                                  
                {
                    // Текст
                    ImGui.TextWrapped("Time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    ImGui.Separator();

                    ImGui.Text("Macca");

                    double l1 = param1.Mass_m;
                    ImGui.InputDouble(":Kr ##label1", ref l1);
                    param1.Mass_m  = l1;
                    ImGui.Separator();

                    ImGui.Text("ridigi");

                    double l2 = param1.Rigidity_k;
                    ImGui.InputDouble("##label2", ref l2);
                    param1.Rigidity_k = l2;
                    ImGui.Separator();

                    bool b1 = false;
                    if (ImGui.Button("= ##butt"))
                    {
                        _dataProcess.Calculate(param1);
                        b1 = true;
                    }
                    if(param1.Rigidity_k != 0 && param1.Mass_m != 0 && b1 == true)
                    {
                        param1.Bat = true;
                    }
                    if (param1.Rigidity_k == 0 || param1.Mass_m == 0)
                    {
                        param1.Bat = false;
                    }
                    if (param1.Bat)
                    {
                        ImGui.Separator();

                        ImGui.Text("Amplitude");
                        float l3 = param1.Amplitude_A;
                        ImGui.SliderFloat(":M ##sl1", ref l3, 0.0f, 100.0f);
                        param1.Amplitude_A = l3;
                    }
                 

                    // Кнопки
                    /* if (ImGui.Button("Knopka 1"))
                     {
                         Console.WriteLine("Nazhata knopka 1!");
                     }
                     ImGui.SameLine();
                     if (ImGui.Button("Knopka 2"))
                     {
                         Console.WriteLine("Nazhata knopka 2!");
                     }

                     // Слайдер
                     ImGui.SliderFloat("Slajder", ref _sliderValue, 0.0f, 100.0f);
                     ImGui.Text($"Znachenie slajdera: {_sliderValue:F2}");

                     // Текстовое поле
                     byte[] buffer2 = System.Text.Encoding.UTF8.GetBytes(_textInput);
                     Array.Resize(ref buffer2, 256);
                     if (ImGui.InputText("Tekst", buffer2, (uint)buffer2.Length))
                     {
                         _textInput = System.Text.Encoding.UTF8.GetString(buffer2).TrimEnd('\0');
                     }

                     // Чекбокс
                     ImGui.Checkbox("Vklyuchit' funkciyu", ref _checkboxValue);
                     if (_checkboxValue)
                     {
                         ImGui.TextColored(new Vector4(0, 1, 0, 1), "Funkciya vklyuchena!");
                     }

                     // Combo box
                     ImGui.Combo("Vybor opcii", ref _comboSelection, _comboItems, _comboItems.Length);
                     ImGui.Text($"Vybrano: {_comboItems[_comboSelection]}");

                     // Цветовой редактор
                     ImGui.ColorEdit3("Cvet", ref _color);
                     ImGui.Text($"RGB: ({_color.X:F2}, {_color.Y:F2}, {_color.Z:F2})");

                     // Прогресс бар
                     float progress = _sliderValue / 100.0f;
                     ImGui.ProgressBar(progress, new Vector2(0, 0), $"{progress * 100:F0}%");

                     // Таблица
                     if (ImGui.BeginTable("Tablitsa", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
                     {
                         ImGui.TableSetupColumn("Kolonka 1");
                         ImGui.TableSetupColumn("Kolonka 2");
                         ImGui.TableSetupColumn("Kolonka 3");
                         ImGui.TableHeadersRow();

                         for (int row = 0; row < 5; row++)
                         {
                             ImGui.TableNextRow();
                             ImGui.TableSetColumnIndex(0);
                             ImGui.Text($"Yachejka {row + 1}-1");
                             ImGui.TableSetColumnIndex(1);
                             ImGui.Text($"Yachejka {row + 1}-2");
                             ImGui.TableSetColumnIndex(2);
                             ImGui.Text($"Yachejka {row + 1}-3");
                         }

                         ImGui.EndTable();
                     }*/
                }
                ImGui.End();

                
            }
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(300, 20));
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(600, 300));
            if (ImGui.Begin("poluchenie dannie", ImGuiWindowFlags.NoMove |
                                                         ImGuiWindowFlags.NoResize |
                                                         ImGuiWindowFlags.NoCollapse))
            {
                if (ImGui.BeginTable("", 3, ImGuiTableFlags.Borders |
                                            ImGuiTableFlags.RowBg))
                {
                    ImGui.TableSetupColumn("period");
                    ImGui.TableSetupColumn("chastota");
                    ImGui.TableSetupColumn("ciklichescaia chastota");
                    ImGui.TableHeadersRow();

                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.Text($"{param1.Period_T} c");
                    ImGui.TableSetColumnIndex(1);
                    ImGui.Text($"{param1.Frequency_v} 1/c");
                    ImGui.TableSetColumnIndex(2);
                    ImGui.Text($"{param1.C_frequency_w} pad/c");
                    
                    ImGui.EndTable();
                }
            }
            ImGui.End();


            // Окно с графиком
            if (_showGraphWindow)
            {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(300, 320));
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(600, 300));
                if (ImGui.Begin("Grafiki", ImGuiWindowFlags.NoMove |
                                           ImGuiWindowFlags.NoResize |
                                           ImGuiWindowFlags.NoCollapse))
                {
                    
                    _dataProcess.Graphs(param1);
                   ImGui.PlotLines("##function_graph", ref param1.Function_data[0], param1.Function_data_l, 0, null, 0f, 100f, new Vector2(550, 250));
                }
                ImGui.End();
            }

            // Окно "О программе"
            if (_requestAboutWindow)
            {
                ImGui.OpenPopup("info");
                _requestAboutWindow = false;
                _showAboutWindow = true;
            }

            if (ImGui.BeginPopupModal("O programme", ref _showAboutWindow, ImGuiWindowFlags.AlwaysAutoResize))
            {
                ImGui.Text("ImGui prilozhenie na C#");
                ImGui.Separator();
                ImGui.Text("Sozdano s ispol'zovaniem:");
                ImGui.BulletText("ImGui.NET");
                ImGui.BulletText("Silk.NET");
                ImGui.BulletText("OpenGL");
                if (ImGui.Button("Zakryt'"))
                {
                    _showAboutWindow = false;
                    ImGui.CloseCurrentPopup();
                }
                ImGui.EndPopup();
            }

            _controller.Render();
        }

        private void OnClosing()
        {
            _controller?.Dispose();
            _gl?.Dispose();
        }

        public void Dispose()
        {
            _controller?.Dispose();
            _gl?.Dispose();
            _window?.Dispose();
        }
    }
}

