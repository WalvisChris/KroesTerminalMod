using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace KroesTerminal
{
    internal class TextJumpAnimation : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private float jumpTime;
        private const float jumpDuration = 0.2f;
        private const float jumpScale = 1.5f;
        private Vector3 originalScale;

        public void StartJump(TextMeshProUGUI target)
        {
            text = target;
            originalScale = transform.localScale;
            jumpTime = jumpDuration;
        }

        void Update()
        {
            if (text == null) return;

            if (jumpTime > 0)
            {
                float t = 1f - (jumpTime / jumpDuration);
                float scale = Mathf.Sin(t * Mathf.PI) * (jumpScale - 1f) + 1f;
                text.transform.localScale = originalScale * scale;
                jumpTime -= Time.deltaTime;

                if (jumpTime <= 0)
                {
                    text.transform.localScale = originalScale;
                    Destroy(this);
                }
            }
        }
    }
}
