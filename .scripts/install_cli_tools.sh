#!/bin/bash

# Verify installation
go version

# Install the CLI tools used in this repository
go install github.com/go-task/task/v3/cmd/task@latest
