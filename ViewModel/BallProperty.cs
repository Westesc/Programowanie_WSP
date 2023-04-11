﻿using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
// Property Class
public class BallProperty : INotifyPropertyChanged {

    private Vector2 position = new Vector2(0, 0);

    public event PropertyChangedEventHandler? PropertyChanged;

    public float X {
        get { return position.X; }
        set { position.X = value; OnPropertyChanged(); }
    }

    public float Y {
        get { return position.Y; }
        set { position.Y = value; OnPropertyChanged(); }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string caller = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
    }

    public void ChangePosition(Vector2 position) {
        this.X = position.X;
        this.Y = position.Y;
	}

}