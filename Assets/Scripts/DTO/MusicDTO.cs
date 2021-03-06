﻿using System.Collections.Generic;

namespace NoteEditor.DTO
{
    public class MusicDTO
    {
        public class EditData
        {
            public string name;
            public int maxBlock;
            public int BPM;
            public int offset;
            public List<Note> notes;
        }

        public class Note
        {
            public int LPB;
            public int num;
            public int block;
            public int type;
            public List<Note> notes;
        }
    }
}
