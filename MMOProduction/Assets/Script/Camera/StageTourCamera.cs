//
// StageTourCamera.cs
//
// Author: Tama
//
// ログイン画面で使用するカメラ（ステージを一周する）
//

using UnityEngine;
using UnityEngine.UI;


public class StageTourCamera : MonoBehaviour
{

    [System.Serializable]
    public struct Lerp
    {
        public Vector3 stat;
        public Vector3 end;
    }


    [SerializeField]
    private WorkingCamera _camera = null;

    [SerializeField]
    private RenderTexture _preRenderTexture = null;
    [SerializeField]
    private RawImage _cameraViewImage = null;

    [SerializeField]
    private Lerp _position = new Lerp();
    [SerializeField]
    private Lerp _rotation = new Lerp();

    [SerializeField]
    private float _updateTime = 1;

    private float _time;


    private void Awake()
    {

        // 開始処理を登録
        _camera.OnStart = (() => {
            this.transform.position = _position.stat;
            this.transform.eulerAngles = _rotation.stat;
            _time = 0;

            _cameraViewImage.texture = _preRenderTexture;
            _cameraViewImage.color = new Color32(255, 255, 255, 255);
        });

        // 更新処理を登録
        _camera.OnUpdate = (() => {
            _time += Time.deltaTime;
            this.transform.eulerAngles = Vector3.Lerp(_rotation.stat, _rotation.end, _time * _updateTime);
            this.transform.position = Vector3.Lerp(_position.stat, _position.end, _time * _updateTime);

            int alpha = Mathf.Abs((int)(255 * Mathf.Clamp01(_time)) - 255);
            _cameraViewImage.color = new Color32(255, 255, 255, (byte)(alpha));

            if (_time >= 1 / _updateTime)
            {
                return WorkingCamera.WorkState.Finised;
            }

            return WorkingCamera.WorkState.Active;
        });
    }
}