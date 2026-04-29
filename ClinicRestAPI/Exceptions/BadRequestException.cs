namespace ClinicRestAPI.Exceptions;

public class BadRequestException(string msg) : Exception(msg){}