import json
from pamqp.commands import Basic


async def send_result(channel, result_message: dict):
    body = json.dumps(result_message).encode("utf-8")

    props = Basic.Properties(
        content_type="application/json",
        delivery_mode=1,
    )

    await channel.basic_publish(
        body=body,
        exchange="",
        routing_key="voice-analysis-result-queue",
        properties=props,
    )
