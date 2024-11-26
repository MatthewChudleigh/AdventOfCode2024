#!/bin/bash

echo "Run Container post-create command..."

git pull --rebase

eval "$(task --completion bash)"
