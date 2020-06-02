using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public bool isOncePerDay;
    public List<Sentence> sentences;
    internal Appointment linkedAppointment;

    internal bool WasHeld { get; set; }

    public Dialogue(bool isOncePerDay, List<Sentence> sentences)
    {
        this.isOncePerDay = isOncePerDay;
        this.sentences = sentences;
    }
}
