import numpy as np
import librosa
import soundfile as sf
import parselmouth
import io
import subprocess
import tempfile
from typing import Tuple, List, Optional


# =========================
# Audio loading & cleanup
# =========================

def convert_to_wav(data: bytes) -> bytes:
    """
    Конвертирует mp3/webm и другие форматы в WAV через ffmpeg.
    Возвращает байты WAV.
    """
    with tempfile.NamedTemporaryFile(suffix=".input", delete=False) as f_in, \
         tempfile.NamedTemporaryFile(suffix=".wav", delete=False) as f_out:

        f_in.write(data)
        f_in.flush()

        cmd = [
            "ffmpeg",
            "-y",
            "-i", f_in.name,
            "-ar", "16000",
            "-ac", "1",
            f_out.name
        ]
        subprocess.run(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE, check=True)

        with open(f_out.name, "rb") as f:
            return f.read()


def load_audio_from_bytes(
    data: bytes,
    target_sr: int = 16000
) -> Tuple[np.ndarray, int]:
    """
    Загружает аудио из bytes, приводит к mono и нужной частоте.
    """
    data = convert_to_wav(data)

    y, sr = sf.read(io.BytesIO(data))

    if y.ndim > 1:
        y = np.mean(y, axis=1)

    if sr != target_sr:
        y = librosa.resample(y, orig_sr=sr, target_sr=target_sr)
        sr = target_sr

    return y.astype(np.float32), sr


def trim_silence(y: np.ndarray, top_db: int = 25) -> np.ndarray:
    """
    Убирает тишину в начале и конце.
    """
    yt, _ = librosa.effects.trim(y, top_db=top_db)
    return yt


# =========================
# Feature extraction
# =========================

def analyze_pitch(
    y: np.ndarray,
    sr: int,
    fmin: int = 75,
    fmax: int = 500
) -> Tuple[Optional[float], Optional[float], Optional[float], Optional[float]]:
    """
    Pitch analysis using YIN
    Returns: mean, std, min, max
    """
    f0 = librosa.yin(y, fmin=fmin, fmax=fmax)
    f0 = f0[~np.isnan(f0)]

    if len(f0) == 0:
        return None, None, None, None

    return (
        float(np.mean(f0)),
        float(np.std(f0)),
        float(np.min(f0)),
        float(np.max(f0))
    )


def analyze_volume(y: np.ndarray) -> Tuple[float, float, float]:
    """
    Mean, std, peak volume in dB
    """
    rms = librosa.feature.rms(y=y)[0]
    db = librosa.amplitude_to_db(rms)

    return (
        float(np.mean(db)),
        float(np.std(db)),
        float(np.max(db))
    )


def analyze_mfcc(
    y: np.ndarray,
    sr: int,
    n_mfcc: int = 13
) -> List[float]:
    """
    MFCC (mean values)
    """
    mfcc = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=n_mfcc)
    return mfcc.mean(axis=1).tolist()


def analyze_speech_rate(y: np.ndarray, sr: int, duration: float) -> float:
    """
    Approximate speech rate (onsets per second)
    """
    onsets = librosa.onset.onset_detect(y=y, sr=sr)
    return float(len(onsets) / max(duration, 1e-6))


def analyze_pauses(y: np.ndarray, top_db: int = 30) -> float:
    """
    Pause ratio (0..1)
    """
    intervals = librosa.effects.split(y, top_db=top_db)
    voiced_samples = sum(end - start for start, end in intervals)
    return float(1.0 - voiced_samples / len(y))


def analyze_jitter_shimmer(
    y: np.ndarray,
    sr: int,
    f0_min: int = 75,
    f0_max: int = 500
) -> Tuple[Optional[float], Optional[float]]:
    """
    Jitter & Shimmer via Praat
    """
    try:
        snd = parselmouth.Sound(y, sr)
        pp = parselmouth.praat.call(
            snd,
            "To PointProcess (periodic, cc)",
            f0_min,
            f0_max
        )

        jitter = parselmouth.praat.call(
            pp,
            "Get jitter (local)",
            0, 0, 0.0001, 0.02, 1.3
        )

        shimmer = parselmouth.praat.call(
            [snd, pp],
            "Get shimmer (local)",
            0, 0, 0.0001, 0.02, 1.3, 1.6
        )

        return float(jitter), float(shimmer)

    except Exception:
        return None, None


# =========================
# Main pipeline
# =========================

def analyze_voice_bytes(data: bytes) -> dict:
    """
    Полный анализ аудио -> dict для JSON
    """
    y, sr = load_audio_from_bytes(data)
    y = trim_silence(y)

    duration = float(len(y) / sr)

    (
        pitch_mean,
        pitch_std,
        pitch_min,
        pitch_max
    ) = analyze_pitch(y, sr)

    volume_mean, volume_std, volume_peak = analyze_volume(y)
    speech_rate = analyze_speech_rate(y, sr, duration)
    pause_ratio = analyze_pauses(y)

    mfcc_mean = analyze_mfcc(y, sr)
    jitter, shimmer = analyze_jitter_shimmer(y, sr)

    return {
        "duration": duration,

        "pitch_mean": pitch_mean,
        "pitch_std": pitch_std,
        "pitch_min": pitch_min,
        "pitch_max": pitch_max,

        "volume_mean_db": volume_mean,
        "volume_std_db": volume_std,
        "volume_peak_db": volume_peak,

        "speech_rate": speech_rate,
        "pause_ratio": pause_ratio,

        "jitter": jitter,
        "shimmer": shimmer,

        "mfcc_mean": mfcc_mean
    }
