using System;

public class FaceletCube
{
    enum Facelet
    {
        U1, U2, U3, U4, U5, U6, U7, U8, U9,
        R1, R2, R3, R4, R5, R6, R7, R8, R9,
        F1, F2, F3, F4, F5, F6, F7, F8, F9,
        D1, D2, D3, D4, D5, D6, D7, D8, D9,
        L1, L2, L3, L4, L5, L6, L7, L8, L9,
        B1, B2, B3, B4, B5, B6, B7, B8, B9
    }
    
    enum Singmaster
    {
        F,  B,  U,  D,  L,  R,  M,  E,  S,
        F_, B_, U_, D_, L_, R_, M_, E_, S_,
        F2, B2, U2, D2, L2, R2, M2, E2, S2,
        f,  b,  u,  d,  l,  r,  x,  y,  z,
        f_, b_, u_, d_, l_, r_, x_, y_, z_,
        f2, b2, u2, d2, l2, r2, x2, y2, z2,
        None
    }

    private Color[] _facelet; 
    Facelet[][] MoveTable = new Facelet[55][]; // 55 Singmaster movements, 54 facelets. Only implemented x, x', y, y', z and z'

    public Color this[int facelet]
    {
        get => _facelet[facelet];
        set => _facelet[facelet] = value;
    }

    public FaceletCube()
	{
        _facelet = new Color[54]; // 3 facelets * 3 facelets * 6 sides = 54 facelets
        for (int i = 0; i < _facelet.Length; i++)
        {
            _facelet[i] = Colors.Gray;
        }
        /*
        MoveTable[(int)Singmaster.F] = new Facelet[]
        {
            Facelet.U1, Facelet.U2, Facelet.U3, Facelet.U4, Facelet.U5, Facelet.U6, Facelet.L9, Facelet.L6, Facelet.L3,
            Facelet.U7, Facelet.R2, Facelet.R3, Facelet.U8, Facelet.R5, Facelet.R6, Facelet.U9, Facelet.R8, Facelet.R9,
            falta terminar!
        };
        MoveTable[(int) Singmaster.x] = [
            Facelet.F1, Facelet.F2, Facelet.F3, Facelet.F4, Facelet.F5, Facelet.F6, Facelet.F7, Facelet.F8, Facelet.F9,
            Facelet.R7, Facelet.R4, Facelet.R1, Facelet.R8, Facelet.R5, Facelet.R2, Facelet.R9, Facelet.R6, Facelet.R3,
            Facelet.D1, Facelet.D2, Facelet.D3, Facelet.D4, Facelet.D5, Facelet.D6, Facelet.D7, Facelet.D8, Facelet.D9,
            Facelet.B9, Facelet.B8, Facelet.B7, Facelet.B6, Facelet.B5, Facelet.B4, Facelet.B3, Facelet.B2, Facelet.B1,
            Facelet.L3, Facelet.L6, Facelet.L9, Facelet.L2, Facelet.L5, Facelet.L8, Facelet.L1, Facelet.L4, Facelet.L7,
            Facelet.U9, Facelet.U8, Facelet.U7, Facelet.U6, Facelet.U5, Facelet.U4, Facelet.U3, Facelet.U2, Facelet.U1
        ];

        MoveTable[(int)Singmaster.x_] = [
            Facelet.B9, Facelet.B8, Facelet.B7, Facelet.B6, Facelet.B5, Facelet.B4, Facelet.B3, Facelet.B2, Facelet.B1,
            Facelet.R3, Facelet.R6, Facelet.R9, Facelet.R2, Facelet.R5, Facelet.R8, Facelet.R1, Facelet.R4, Facelet.R7,
            Facelet.U1, Facelet.U2, Facelet.U3, Facelet.U4, Facelet.U5, Facelet.U6, Facelet.U7, Facelet.U8, Facelet.U9,
            Facelet.F1, Facelet.F2, Facelet.F3, Facelet.F4, Facelet.F5, Facelet.F6, Facelet.F7, Facelet.F8, Facelet.F9,
            Facelet.L7, Facelet.L4, Facelet.L1, Facelet.L8, Facelet.L5, Facelet.L2, Facelet.L9, Facelet.L6, Facelet.L3,
            Facelet.D9, Facelet.D8, Facelet.D7, Facelet.D6, Facelet.D5, Facelet.D4, Facelet.D3, Facelet.D2, Facelet.D1
        ];

        MoveTable[(int)Singmaster.y] = [
            Facelet.U7, Facelet.U4, Facelet.U1, Facelet.U8, Facelet.U5, Facelet.U2, Facelet.U9, Facelet.U6, Facelet.U3,
            Facelet.B1, Facelet.B2, Facelet.B3, Facelet.B4, Facelet.B5, Facelet.B6, Facelet.B7, Facelet.B8, Facelet.B9,
            Facelet.R1, Facelet.R2, Facelet.R3, Facelet.R4, Facelet.R5, Facelet.R6, Facelet.R7, Facelet.R8, Facelet.R9,
            Facelet.D3, Facelet.D6, Facelet.D9, Facelet.D2, Facelet.D5, Facelet.D8, Facelet.D1, Facelet.D4, Facelet.D7,
            Facelet.F1, Facelet.F2, Facelet.F3, Facelet.F4, Facelet.F5, Facelet.F6, Facelet.F7, Facelet.F8, Facelet.F9,
            Facelet.L1, Facelet.L2, Facelet.L3, Facelet.L4, Facelet.L5, Facelet.L6, Facelet.L7, Facelet.L8, Facelet.L9
        ];

        MoveTable[(int)Singmaster.y_] = [
            Facelet.U3, Facelet.U6, Facelet.U9, Facelet.U2, Facelet.U5, Facelet.U8, Facelet.U1, Facelet.U4, Facelet.U7,
            Facelet.F1, Facelet.F2, Facelet.F3, Facelet.F4, Facelet.F5, Facelet.F6, Facelet.F7, Facelet.F8, Facelet.F9,
            Facelet.L1, Facelet.L2, Facelet.L3, Facelet.L4, Facelet.L5, Facelet.L6, Facelet.L7, Facelet.L8, Facelet.L9,
            Facelet.D7, Facelet.D4, Facelet.D1, Facelet.D8, Facelet.D5, Facelet.D2, Facelet.D9, Facelet.D6, Facelet.D3,
            Facelet.B1, Facelet.B2, Facelet.B3, Facelet.B4, Facelet.B5, Facelet.B6, Facelet.B7, Facelet.B8, Facelet.B9,
            Facelet.R1, Facelet.R2, Facelet.R3, Facelet.R4, Facelet.R5, Facelet.R6, Facelet.R7, Facelet.R8, Facelet.R9
        ];

        MoveTable[(int)Singmaster.z] = [
            Facelet.L7, Facelet.L4, Facelet.L1, Facelet.L8, Facelet.L5, Facelet.L2, Facelet.L9, Facelet.L6, Facelet.L3,
            Facelet.U7, Facelet.U4, Facelet.U1, Facelet.U8, Facelet.U5, Facelet.U2, Facelet.U9, Facelet.U6, Facelet.U3,
            Facelet.F7, Facelet.F4, Facelet.F1, Facelet.F8, Facelet.F5, Facelet.F2, Facelet.F9, Facelet.F6, Facelet.F3,
            Facelet.R7, Facelet.R4, Facelet.R1, Facelet.R8, Facelet.R5, Facelet.R2, Facelet.R9, Facelet.R6, Facelet.R3,
            Facelet.D7, Facelet.D4, Facelet.D1, Facelet.D8, Facelet.D5, Facelet.D2, Facelet.D9, Facelet.D6, Facelet.D3,
            Facelet.B3, Facelet.B6, Facelet.B9, Facelet.B2, Facelet.B5, Facelet.B8, Facelet.B1, Facelet.B4, Facelet.B7
        ];

        MoveTable[(int)Singmaster.z_] = [
            Facelet.R3, Facelet.R6, Facelet.R9, Facelet.R2, Facelet.R5, Facelet.R8, Facelet.R1, Facelet.R4, Facelet.R7,
            Facelet.D3, Facelet.D6, Facelet.D9, Facelet.D2, Facelet.D5, Facelet.D8, Facelet.D1, Facelet.D4, Facelet.D7,
            Facelet.F3, Facelet.F6, Facelet.F9, Facelet.F2, Facelet.F5, Facelet.F8, Facelet.F1, Facelet.F4, Facelet.F7,
            Facelet.L3, Facelet.L6, Facelet.L9, Facelet.L2, Facelet.L5, Facelet.L8, Facelet.L1, Facelet.L4, Facelet.L7,
            Facelet.U3, Facelet.U6, Facelet.U9, Facelet.U2, Facelet.U5, Facelet.U8, Facelet.U1, Facelet.U4, Facelet.U7,
            Facelet.B7, Facelet.B4, Facelet.B1, Facelet.B8, Facelet.B5, Facelet.B2, Facelet.B9, Facelet.B6, Facelet.B3
        ];
        */
        
    }

    public string ToStr()
    {
        string output = string.Empty;

        Color U_color = _facelet[(int) Facelet.U5];
        Color R_color = _facelet[(int) Facelet.R5];
        Color F_color = _facelet[(int) Facelet.F5];
        Color D_color = _facelet[(int) Facelet.D5];
        Color L_color = _facelet[(int) Facelet.L5];
        Color B_color = _facelet[(int) Facelet.B5];

        foreach (Color c in _facelet)
        {
            if (c == _facelet[(int)Facelet.U5]) {
                output += 'U';
            } else if (c == _facelet[(int)Facelet.R5]) {
                output += 'R';
            } else if (c == _facelet[(int)Facelet.F5]) {
                output += 'F';
            } else if (c == _facelet[(int)Facelet.D5]) {
                output += 'D';
            } else if (c == _facelet[(int)Facelet.L5]) {
                output += 'L';
            } else if (c == _facelet[(int)Facelet.B5]) {
                output += 'B';
            } else {
                output += '?';
            }
        }

        return output;
    }


}
