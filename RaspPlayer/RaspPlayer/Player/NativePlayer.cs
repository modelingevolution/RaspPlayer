using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using RaspPlayer.Platforms.Android;
using View = Android.Views.View;

namespace RaspPlayer.Player;

[Register("me/player/NativePlayer", DoNotGenerateAcw = true)]
public class NativePlayer : Java.Lang.Object
{
    private static readonly IntPtr ClassReference = JNIEnv.FindClass("me/player/NativePlayer");

    public NativePlayer()
    {
    }

    public NativePlayer(IntPtr handle, JniHandleOwnership transfer)
        : base(handle, transfer)
    {
    }

    public bool IsVideoOffline { get; set; }
    protected override Type ThresholdType => typeof(NativePlayer);
    protected override IntPtr ThresholdClass => ClassReference;

    #region Open

    private static IntPtr _openId;

    private const string OpenSignature = "(Ljava/lang/String;Ljava/lang/String;Ljava/lang/String;)J";

    [Register("open", OpenSignature, nameof(GetOpenHandler))]
    public virtual long Open(string url, string parameters, string deviceName)
    {
        if (_openId == IntPtr.Zero)
        {
            _openId = JNIEnv.GetMethodID(ClassReference, "open", OpenSignature);
        }

        var methodParameters = new JValue[]
        {
            url.ToJValue(), parameters.ToJValue(), deviceName.ToJValue()
        };

        if (GetType() == ThresholdType)
        {
            return JNIEnv.CallLongMethod(Handle, _openId, methodParameters);
        }

        return JNIEnv.CallNonvirtualLongMethod(Handle, ThresholdClass, _openId, methodParameters);
    }

    private static Delegate _openDelegate;

    private static Delegate GetOpenHandler()
    {
        return _openDelegate ??=
            JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, string, string, string, long>)NativeOpen);
    }

    private static long NativeOpen(IntPtr jniEnv, IntPtr refThis, string url, string parameters, string deviceName)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        return nativePlayerObject!.Open(url, parameters, deviceName);
    }

    #endregion

    #region Close

    private static IntPtr _closeId;
    private const string CloseSignature = "()V";

    [Register("close", CloseSignature, nameof(GetCloseHandler))]
    public virtual void Close()
    {
        if (_closeId == IntPtr.Zero)
        {
            _closeId = JNIEnv.GetMethodID(ClassReference, "close", CloseSignature);
        }

        if (GetType() == ThresholdType)
        {
            JNIEnv.CallVoidMethod(Handle, _closeId);
            return;
        }

        JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, _closeId);
    }

    private static Delegate _closeDelegate;

    private static Delegate GetCloseHandler()
    {
        return _closeDelegate ??= JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr>)NativeClose);
    }

    private static void NativeClose(IntPtr jniEnv, IntPtr refThis)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        nativePlayerObject!.Close();
    }

    #endregion

    #region Finalize

    private static IntPtr _finalizeId;
    private const string FinalizeSignature = "()V";

    [Register("finalize", FinalizeSignature, nameof(GetFinalizeHandler))]
    public virtual void FinalizePlayer()
    {
        if (_finalizeId == IntPtr.Zero)
        {
            _finalizeId = JNIEnv.GetMethodID(ClassReference, "finalize", FinalizeSignature);
        }

        if (GetType() == ThresholdType)
        {
            JNIEnv.CallVoidMethod(Handle, _finalizeId);
            return;
        }

        JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, _finalizeId);
    }

    private static Delegate _finalizeDelegate;

    private static Delegate GetFinalizeHandler()
    {
        return _finalizeDelegate ??= JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr>)NativeFinalize);
    }

    private static void NativeFinalize(IntPtr jniEnv, IntPtr refThis)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        nativePlayerObject!.FinalizePlayer();
    }

    #endregion

    #region Seek

    private static IntPtr _seekId;
    private const string SeekSignature = "(J)V";

    [Register("seek", SeekSignature, nameof(GetSeekHandler))]
    public virtual void Seek(long ms)
    {
        if (_seekId == IntPtr.Zero)
        {
            _seekId = JNIEnv.GetMethodID(ClassReference, "seek", SeekSignature);
        }

        var methodParameters = new JValue[]
        {
            new JValue(ms)
        };

        if (GetType() == ThresholdType)
        {
            JNIEnv.CallVoidMethod(Handle, _seekId, methodParameters);
            return;
        }

        JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, _seekId, methodParameters);
    }

    private static Delegate _seekDelegate;

    private static Delegate GetSeekHandler()
    {
        return _seekDelegate ??= JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, long>)NativeSeek);
    }

    private static void NativeSeek(IntPtr jniEnv, IntPtr refThis, long ms)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        nativePlayerObject!.Seek(ms);
    }

    #endregion

    #region SetParam

    private static IntPtr _setParamId;
    private const string SetParamSignature = "(IJ)V";

    [Register("setParam", SetParamSignature, nameof(GetSetParamHandler))]
    public virtual void SetParam(int id, long value)
    {
        if (_setParamId == IntPtr.Zero)
        {
            _setParamId = JNIEnv.GetMethodID(ClassReference, "setParam", SetParamSignature);
        }

        var methodParameters = new JValue[]
        {
            new JValue(id), new JValue(value)
        };

        if (GetType() == ThresholdType)
        {
            JNIEnv.CallVoidMethod(Handle, _setParamId, methodParameters);
            return;
        }

        JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, _setParamId, methodParameters);
    }

    private static Delegate _setParamDelegate;

    private static Delegate GetSetParamHandler()
    {
        return _setParamDelegate ??= JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, int, long>)NativeSetParam);
    }

    private static void NativeSetParam(IntPtr jniEnv, IntPtr refThis, int id, long value)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        nativePlayerObject!.SetParam(id, value);
    }

    #endregion

    #region GetParam

    private static IntPtr _getParamId;
    private const string GetParamSignature = "(I)J";

    [Register("getParam", GetParamSignature, nameof(GetGetParamHandler))]
    public virtual long GetParam(int id)
    {
        if (_getParamId == IntPtr.Zero)
        {
            _getParamId = JNIEnv.GetMethodID(ClassReference, "getParam", GetParamSignature);
        }

        var methodParameters = new JValue[]
        {
            new JValue(id)
        };

        if (GetType() == ThresholdType)
        {
            return JNIEnv.CallLongMethod(Handle, _getParamId, methodParameters);
        }

        return JNIEnv.CallNonvirtualLongMethod(Handle, ThresholdClass, _getParamId, methodParameters);
    }

    private static Delegate _getParamDelegate;

    private static Delegate GetGetParamHandler()
    {
        return _getParamDelegate ??= JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, int, long>)NativeGetParam);
    }

    private static long NativeGetParam(IntPtr jniEnv, IntPtr refThis, int id)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        return nativePlayerObject!.GetParam(id);
    }

    #endregion

    #region Play

    private static IntPtr _playId;
    private const string PlaySignature = "()V";

    [Register("play", PlaySignature, nameof(GetPlayHandler))]
    public virtual void Play()
    {
        if (_playId == IntPtr.Zero)
        {
            _playId = JNIEnv.GetMethodID(ClassReference, "play", PlaySignature);
        }

        if (GetType() == ThresholdType)
        {
            JNIEnv.CallVoidMethod(Handle, _playId);
            return;
        }

        JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, _playId);
    }

    private static Delegate _playDelegate;

    private static Delegate GetPlayHandler()
    {
        return _playDelegate ??= JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr>)NativePlay);
    }

    private static void NativePlay(IntPtr jniEnv, IntPtr refThis)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        nativePlayerObject!.Play();
    }

    #endregion

    #region Pause

    private static IntPtr _pauseId;
    private const string PauseSignature = "()V";

    [Register("pause", PauseSignature, nameof(GetPauseHandler))]
    public virtual void Pause()
    {
        if (_pauseId == IntPtr.Zero)
        {
            _pauseId = JNIEnv.GetMethodID(ClassReference, "pause", PauseSignature);
        }

        if (GetType() == ThresholdType)
        {
            JNIEnv.CallVoidMethod(Handle, _pauseId);
            return;
        }

        JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, _pauseId);
    }

    private static Delegate _pauseDelegate;

    private static Delegate GetPauseHandler()
    {
        return _pauseDelegate ??= JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr>)NativePause);
    }

    private static void NativePause(IntPtr jniEnv, IntPtr refThis)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        nativePlayerObject!.Pause();
    }

    #endregion

    #region InitVideoSize

    private static IntPtr _initVideoSizeId;
    private const string InitVideoSizeSignature = "(IILandroid/view/View;)Z";

    [Register("initVideoSize", InitVideoSizeSignature, nameof(GetInitVideoSizeHandler))]
    public virtual bool InitVideoSize(int rw, int rh, Android.Views.View v)
    {
        if (_initVideoSizeId == IntPtr.Zero)
        {
            _initVideoSizeId = JNIEnv.GetMethodID(ClassReference, "initVideoSize", InitVideoSizeSignature);
        }

        var methodParameters = new JValue[]
        {
            new JValue(rw),
            new JValue(rh),
            new JValue(v)
        };

        if (GetType() == ThresholdType)
        {
            return JNIEnv.CallBooleanMethod(Handle, _initVideoSizeId, methodParameters);
        }

        return JNIEnv.CallNonvirtualBooleanMethod(Handle, ThresholdClass, _initVideoSizeId, methodParameters);
    }

    private static Delegate _initVideoSizeDelegate;

    private static Delegate GetInitVideoSizeHandler()
    {
        return _initVideoSizeDelegate ??=
            JNINativeWrapper.CreateDelegate(
                (Func<IntPtr, IntPtr, int, int, Android.Views.View, bool>)NativeInitVideoSize);
    }

    private static bool NativeInitVideoSize(IntPtr jniEnv, IntPtr refThis, int rw, int rh, Android.Views.View v)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        return nativePlayerObject!.InitVideoSize(rw, rh, v);
    }

    #endregion

    #region SetDisplaySurface

    private static IntPtr _setDisplaySurfaceId;
    private const string SetDisplaySurfaceSignature = "(Landroid/view/Surface;)V";

    [Register("setDisplaySurface", SetDisplaySurfaceSignature, nameof(GetSetDisplaySurfaceHandler))]
    public virtual void SetDisplaySurface(Android.Views.Surface surface)
    {
        if (_setDisplaySurfaceId == IntPtr.Zero)
        {
            _setDisplaySurfaceId = JNIEnv.GetMethodID(ClassReference, "setDisplaySurface", SetDisplaySurfaceSignature);
        }

        var methodParameters = new JValue[]
        {
            new JValue(surface)
        };

        if (GetType() == ThresholdType)
        {
            JNIEnv.CallVoidMethod(Handle, _setDisplaySurfaceId, methodParameters);
            return;
        }

        JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, _setDisplaySurfaceId, methodParameters);
    }

    private static Delegate _setDisplaySurfaceDelegate;

    private static Delegate GetSetDisplaySurfaceHandler()
    {
        return _setDisplaySurfaceDelegate ??=
            JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, Android.Views.Surface>)NativeSetDisplaySurface);
    }

    private static void NativeSetDisplaySurface(IntPtr jniEnv, IntPtr refThis, Android.Views.Surface surface)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        nativePlayerObject!.SetDisplaySurface(surface);
    }

    #endregion

    #region SetDisplayTexture

    private static IntPtr _setDisplayTextureId;
    private const string SetDisplayTextureSignature = "(Landroid/graphics/SurfaceTexture;)V";

    [Register("setDisplayTexture", SetDisplayTextureSignature, nameof(GetSetDisplayTextureHandler))]
    public virtual void SetDisplayTexture(Android.Graphics.SurfaceTexture texture)
    {
        if (_setDisplayTextureId == IntPtr.Zero)
        {
            _setDisplayTextureId = JNIEnv.GetMethodID(ClassReference, "setDisplayTexture", SetDisplayTextureSignature);
        }

        var methodParameters = new JValue[]
        {
            new JValue(texture)
        };

        if (GetType() == ThresholdType)
        {
            JNIEnv.CallVoidMethod(Handle, _setDisplayTextureId, methodParameters);
            return;
        }

        JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, _setDisplayTextureId, methodParameters);
    }

    private static Delegate _setDisplayTextureDelegate;

    private static Delegate GetSetDisplayTextureHandler()
    {
        return _setDisplayTextureDelegate ??=
            JNINativeWrapper.CreateDelegate(
                (Action<IntPtr, IntPtr, Android.Graphics.SurfaceTexture>)NativeSetDisplayTexture);
    }

    private static void NativeSetDisplayTexture(IntPtr jniEnv, IntPtr refThis, Android.Graphics.SurfaceTexture texture)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        nativePlayerObject!.SetDisplayTexture(texture);
    }

    #endregion

    #region SetPlayerMesssageHandler

    private static IntPtr _setPlayerMessageHandlerId;
    private const string SetPlayerMessageHandlerSignature = "(Landroid/os/Handler;)V";

    [Register("setPlayerMsgHandler", SetPlayerMessageHandlerSignature, nameof(GetSetPlayerMessageHandlerHandler))]
    public virtual void SetPlayerMessageHandler(Android.OS.Handler h)
    {
        if (_setPlayerMessageHandlerId == IntPtr.Zero)
        {
            _setPlayerMessageHandlerId =
                JNIEnv.GetMethodID(ClassReference, "setPlayerMsgHandler", SetPlayerMessageHandlerSignature);
        }

        var methodParameters = new JValue[]
        {
            new JValue(h)
        };

        if (GetType() == ThresholdType)
        {
            JNIEnv.CallVoidMethod(Handle, _setPlayerMessageHandlerId, methodParameters);
            return;
        }

        JNIEnv.CallNonvirtualVoidMethod(Handle, ThresholdClass, _setPlayerMessageHandlerId, methodParameters);
    }

    private static Delegate _setPlayerMessageHandlerDelegate;

    private static Delegate GetSetPlayerMessageHandlerHandler()
    {
        return _setPlayerMessageHandlerDelegate ??=
            JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, Android.OS.Handler>)NativeSetPlayerMessageHandler);
    }

    private static void NativeSetPlayerMessageHandler(IntPtr jniEnv, IntPtr refThis, Android.OS.Handler h)
    {
        var nativePlayerObject = GetObject<NativePlayer>(refThis, JniHandleOwnership.DoNotTransfer);
        nativePlayerObject!.SetPlayerMessageHandler(h);
    }

    #endregion
}

public class ManagedNativePlayer : NativePlayer
{
    public override long Open(string url, string parameters, string deviceName)
    {
        return 0L;
    }

    public override void Seek(long ms)
    {
    }

    public override void SetParam(int id, long value)
    {
    }

    public override void FinalizePlayer()
    {
    }

    public override void Close()
    {
    }

    public override void Play()
    {
    }

    public override void Pause()
    {
    }

    public override long GetParam(int id)
    {
        return 0L;
    }

    public override bool InitVideoSize(int rw, int rh, View v)
    {
        return false;
    }

    public override void SetDisplaySurface(Surface surface)
    {
    }

    public override void SetDisplayTexture(SurfaceTexture texture)
    {
    }

    public override void SetPlayerMessageHandler(Handler h)
    {
    }
}