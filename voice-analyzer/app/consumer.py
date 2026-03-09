import asyncio
import json
import base64
import math
import os

from aio_pika import (
    connect_robust,
    IncomingMessage,
    ExchangeType,
)
from analysis import analyze_voice_bytes
from models import AudioChunkReceived
from publisher import send_result

RABBITMQ_HOST = os.getenv("RABBITMQ_HOST", "localhost")
RABBIT_URL = f"amqp://guest:guest@{RABBITMQ_HOST}/"
EXCHANGE_NAME = "voice-analysis"
QUEUE_NAME = "voice-analysis-python"


def replace_nan(obj):
    if isinstance(obj, dict):
        return {k: replace_nan(v) for k, v in obj.items()}
    elif isinstance(obj, list):
        return [replace_nan(x) for x in obj]
    elif isinstance(obj, float) and math.isnan(obj):
        return 0.0
    return obj


async def handle_message(message: IncomingMessage):
    async with message.process():
        payload = json.loads(message.body)

        if isinstance(payload, dict) and "message" in payload:
            payload = payload["message"]

        audio_msg = AudioChunkReceived(**payload)

        audio_bytes = base64.b64decode(audio_msg.data)
        analysis = analyze_voice_bytes(audio_bytes)

        result_message = {
            "sessionId": str(audio_msg.sessionId),
            "userId": payload["userId"],
            "duration": analysis["duration"],
            "pitchMean": analysis["pitch_mean"],
            "pitchStd": analysis["pitch_std"],
            "pitchMin": analysis["pitch_min"],
            "pitchMax": analysis["pitch_max"],
            "volumeMeanDb": analysis["volume_mean_db"],
            "volumeStdDb": analysis["volume_std_db"],
            "volumePeakDb": analysis["volume_peak_db"],
            "speechRate": analysis["speech_rate"],
            "pauseRatio": analysis["pause_ratio"],
            "jitter": analysis["jitter"],
            "shimmer": analysis["shimmer"],
            "mfccMean": analysis["mfcc_mean"],
        }

        result_message = replace_nan(result_message)

        await send_result(message.channel, result_message)


async def main():
    connection = await connect_robust(RABBIT_URL)

    channel = await connection.channel()
    await channel.set_qos(prefetch_count=1)

    exchange = await channel.declare_exchange(
        EXCHANGE_NAME,
        ExchangeType.FANOUT,
        durable=True,
    )

    queue = await channel.declare_queue(
        QUEUE_NAME,
        durable=True,
    )

    await queue.bind(exchange)
    await queue.consume(handle_message)

    print("Waiting for voice analysis requests...")
    await asyncio.Future()


if __name__ == "__main__":
    asyncio.run(main())
