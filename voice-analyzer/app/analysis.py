import numpy as np
import librosa
import soundfile as sf
import parselmouth
import io
from typing import Tuple, List
import subprocess
import tempfile


# =========================
# Audio loading & cleanup
# =========================

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


def trim_silence(
    y: np.ndarray,
    top_db: int = 25
) -> np.ndarray:
    """
    Убирает тишину в начале и конце.
    """
    yt, _ = librosa.effects.trim(y, top_db=top_db)
    return yt


# =========================
# Acoustic features
# =========================

def analyze_pitch(
    y: np.ndarray,
    sr: int
) -> Tuple[float | None, float | None, float | None]:
    """
    Средний, минимальный и максимальный pitch (Hz)
    """
    pitches, mags = librosa.piptrack(y=y, sr=sr)

    valid = pitches[mags > np.percentile(mags, 90)]
    valid = valid[valid > 0]

    if len(valid) == 0:
        return None, None, None

    return float(valid.mean()), float(valid.min()), float(valid.max())


def analyze_volume_db(y: np.ndarray) -> float:
    """
    Средняя громкость в dB
    """
    rms = librosa.feature.rms(y=y)[0]
    return float(librosa.amplitude_to_db(np.mean(rms)))


def analyze_mfcc(
    y: np.ndarray,
    sr: int,
    n_mfcc: int = 13
) -> List[float]:
    """
    MFCC (среднее значение)
    """
    mfcc = librosa.feature.mfcc(y=y, sr=sr, n_mfcc=n_mfcc)
    return mfcc.mean(axis=1).tolist()


# =========================
# Voice stability (Praat)
# =========================

def analyze_jitter_shimmer(
    y: np.ndarray,
    sr: int,
    f0_min: int = 75,
    f0_max: int = 500
) -> Tuple[float | None, float | None]:
    """
    Jitter & Shimmer (Praat)
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

    mean_pitch, pitch_min, pitch_max = analyze_pitch(y, sr)
    volume_db = analyze_volume_db(y)
    mfcc_mean = analyze_mfcc(y, sr)
    jitter, shimmer = analyze_jitter_shimmer(y, sr)

    return {
        "duration": duration,
        "mean_pitch": mean_pitch,
        "pitch_min": pitch_min,
        "pitch_max": pitch_max,
        "volume_db": volume_db,
        "jitter": jitter,
        "shimmer": shimmer,
        "mfcc_mean": mfcc_mean
    }

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
            "-y",  # overwrite
            "-i", f_in.name,
            "-ar", "16000",  # частота дискретизации
            "-ac", "1",      # mono
            f_out.name
        ]
        subprocess.run(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE, check=True)

        with open(f_out.name, "rb") as f:
            return f.read()
