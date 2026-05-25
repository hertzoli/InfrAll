using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace GerenciadorSistemas.Services
{
    internal sealed class VirtualKeyboardService
    {
        private const int InputKeyboard = 1;
        private const uint KeyEventFExtendedKey = 0x0001;
        private const uint KeyEventFKeyUp = 0x0002;
        private const uint KeyEventFUnicode = 0x0004;
        private const ushort VirtualKeyBack = 0x08;
        private const ushort VirtualKeyTab = 0x09;
        private const ushort VirtualKeyReturn = 0x0D;
        private const ushort VirtualKeyShift = 0x10;
        private const ushort VirtualKeyControl = 0x11;
        private const ushort VirtualKeyMenu = 0x12;
        private const ushort VirtualKeyNumpad0 = 0x60;
        private const int IntervaloEntreTeclasMs = 8;

        public void EnviarTexto(string texto, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(texto))
                return;

            for (int i = 0; i < texto.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                char caractere = texto[i];
                if (caractere == '\r')
                {
                    EnviarTeclaVirtual(VirtualKeyReturn);
                    if (i + 1 < texto.Length && texto[i + 1] == '\n')
                        i++;
                }
                else if (caractere == '\n')
                {
                    EnviarTeclaVirtual(VirtualKeyReturn);
                }
                else if (caractere == '\t')
                {
                    EnviarTeclaVirtual(VirtualKeyTab);
                }
                else if (caractere == '\b')
                {
                    EnviarTeclaVirtual(VirtualKeyBack);
                }
                else
                {
                    EnviarCaractere(caractere);
                }

                if (IntervaloEntreTeclasMs > 0)
                    Thread.Sleep(IntervaloEntreTeclasMs);
            }
        }

        private static void EnviarCaractere(char caractere)
        {
            if (caractere >= 32 && caractere <= 126 && TryEnviarCaracterePorTeclaVirtual(caractere))
                return;

            if (caractere > 126 && caractere <= 255)
            {
                EnviarCaracterePorAltNumpad(caractere);
                return;
            }

            EnviarCaractereUnicode(caractere);
        }

        private static bool TryEnviarCaracterePorTeclaVirtual(char caractere)
        {
            short conversao = VkKeyScanEx(caractere, GetKeyboardLayout(0));
            if (conversao == -1)
                return false;

            ushort virtualKey = (ushort)(conversao & 0xFF);
            int estadoShift = (conversao >> 8) & 0xFF;

            if ((estadoShift & ~7) != 0)
                return false;

            ushort[] modificadores = ObterModificadores(estadoShift);
            EnviarTeclaVirtual(virtualKey, modificadores);
            return true;
        }

        private static ushort[] ObterModificadores(int estadoShift)
        {
            ushort[] modificadores = new ushort[ContarModificadores(estadoShift)];
            int indice = 0;

            if ((estadoShift & 1) != 0)
                modificadores[indice++] = VirtualKeyShift;

            if ((estadoShift & 2) != 0)
                modificadores[indice++] = VirtualKeyControl;

            if ((estadoShift & 4) != 0)
                modificadores[indice++] = VirtualKeyMenu;

            return modificadores;
        }

        private static int ContarModificadores(int estadoShift)
        {
            int total = 0;

            if ((estadoShift & 1) != 0)
                total++;

            if ((estadoShift & 2) != 0)
                total++;

            if ((estadoShift & 4) != 0)
                total++;

            return total;
        }

        private static void EnviarCaracterePorAltNumpad(char caractere)
        {
            string codigo = ((int)caractere).ToString("0000");
            INPUT[] inputs = new INPUT[2 + (codigo.Length * 2)];
            int indice = 0;

            inputs[indice++] = CriarInputTeclaVirtual(VirtualKeyMenu, 0);

            foreach (char digito in codigo)
            {
                ushort virtualKey = (ushort)(VirtualKeyNumpad0 + (digito - '0'));
                inputs[indice++] = CriarInputTeclaVirtual(virtualKey, 0);
                inputs[indice++] = CriarInputTeclaVirtual(virtualKey, KeyEventFKeyUp);
            }

            inputs[indice] = CriarInputTeclaVirtual(VirtualKeyMenu, KeyEventFKeyUp);
            EnviarInputs(inputs);
        }

        private static void EnviarCaractereUnicode(char caractere)
        {
            INPUT[] inputs = new INPUT[2];
            inputs[0] = CriarInputUnicode(caractere, 0);
            inputs[1] = CriarInputUnicode(caractere, KeyEventFKeyUp);
            EnviarInputs(inputs);
        }

        private static void EnviarTeclaVirtual(ushort virtualKey)
        {
            EnviarTeclaVirtual(virtualKey, new ushort[0]);
        }

        private static void EnviarTeclaVirtual(ushort virtualKey, ushort[] modificadores)
        {
            modificadores = modificadores ?? new ushort[0];

            INPUT[] inputs = new INPUT[(modificadores.Length * 2) + 2];
            int indice = 0;

            for (int i = 0; i < modificadores.Length; i++)
                inputs[indice++] = CriarInputTeclaVirtual(modificadores[i], 0);

            inputs[indice++] = CriarInputTeclaVirtual(virtualKey, 0);
            inputs[indice++] = CriarInputTeclaVirtual(virtualKey, KeyEventFKeyUp);

            for (int i = modificadores.Length - 1; i >= 0; i--)
                inputs[indice++] = CriarInputTeclaVirtual(modificadores[i], KeyEventFKeyUp);

            EnviarInputs(inputs);
        }

        private static INPUT CriarInputUnicode(char caractere, uint flagsAdicionais)
        {
            INPUT input = new INPUT();
            input.Type = InputKeyboard;
            input.Data.KeyboardInput.Scan = caractere;
            input.Data.KeyboardInput.Flags = KeyEventFUnicode | flagsAdicionais;
            return input;
        }

        private static INPUT CriarInputTeclaVirtual(ushort virtualKey, uint flags)
        {
            INPUT input = new INPUT();
            input.Type = InputKeyboard;
            input.Data.KeyboardInput.VirtualKey = virtualKey;
            input.Data.KeyboardInput.Scan = (ushort)(MapVirtualKey(virtualKey, 0) & 0xFF);
            input.Data.KeyboardInput.Flags = flags | (EhTeclaEstendida(virtualKey) ? KeyEventFExtendedKey : 0);
            return input;
        }

        private static bool EhTeclaEstendida(ushort virtualKey)
        {
            return virtualKey == 0x21
                || virtualKey == 0x22
                || virtualKey == 0x23
                || virtualKey == 0x24
                || virtualKey == 0x25
                || virtualKey == 0x26
                || virtualKey == 0x27
                || virtualKey == 0x28
                || virtualKey == 0x2D
                || virtualKey == 0x2E
                || virtualKey == 0x6F
                || virtualKey == 0x90
                || virtualKey == 0xA3
                || virtualKey == 0xA5;
        }

        private static void EnviarInputs(INPUT[] inputs)
        {
            int tamanhoInput = Marshal.SizeOf(typeof(INPUT));
            uint enviados = SendInput((uint)inputs.Length, inputs, tamanhoInput);

            if (enviados != inputs.Length)
            {
                int codigoErro = Marshal.GetLastWin32Error();
                string mensagem = codigoErro == 0
                    ? "O Windows nao aceitou o envio de teclas pelo teclado virtual."
                    : "O Windows recusou o envio de teclas pelo teclado virtual.";

                throw new Win32Exception(codigoErro, string.Format(
                    "{0} Teclas enviadas: {1}/{2}. Tamanho INPUT: {3}.",
                    mensagem,
                    enviados,
                    inputs.Length,
                    tamanhoInput));
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(ushort uCode, uint uMapType);

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public int Type;
            public INPUTUNION Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUTUNION
        {
            [FieldOffset(0)]
            public MOUSEINPUT MouseInput;

            [FieldOffset(0)]
            public KEYBDINPUT KeyboardInput;

            [FieldOffset(0)]
            public HARDWAREINPUT HardwareInput;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT
        {
            public uint Message;
            public ushort ParamL;
            public ushort ParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort VirtualKey;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }
    }
}
