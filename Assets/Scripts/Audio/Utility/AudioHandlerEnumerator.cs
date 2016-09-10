using UnityEngine;
using System.Collections;
using System;

namespace KLFrame
{
    public class AudioHandlerEnumerator : IEnumerator
    {

        private AudioData[] atts;
        private int index = -1;

        public AudioHandlerEnumerator(AudioData[] arr)
        {
            atts = arr;
            for (int i = 0; i < arr.Length; i++)
            {
                atts[i] = arr[i];
            }
        }
        public object Current
        {
            get
            {
                if (index == -1)
                {
                    throw new InvalidOperationException();
                }
                if (index >= atts.Length)
                {
                    throw new InvalidOperationException();
                }
                return atts[index];
            }
        }

        public bool MoveNext()
        {
            if (index < atts.Length - 1)
            {
                index++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            index = -1;
        }


    }
}