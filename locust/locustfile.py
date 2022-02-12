import time
import uuid

from locust import HttpUser, task, between

class QuickstartUser(HttpUser):
    # wait_time = between(1, 2.5)

    @task
    def calculation_execute(self):
        random_guid = str(uuid.uuid4())
        self.client.post("/calculation/" + random_guid + "/execute", json={"expression": "A + 1", "runtime": "Java", "parameters": [{"alias": "A", "value": "1", "type": "Double"}]})

    # @task(3)
    # def view_items(self):
    #     for item_id in range(10):
    #         self.client.get(f"/item?id={item_id}", name="/item")
    #         time.sleep(1)

    # def on_start(self):
    #     self.client.post("/login", json={"username":"foo", "password":"bar"})