﻿namespace Application.Exceptions;

public abstract class NotFoundException(string message) : Exception(message);