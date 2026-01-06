using System.Text.Json.Serialization;

namespace BibliotekaAPI.Dtos;

public record EgzemplarzDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("bookId")] int BookId
);

public record EgzemplarzCreateDto(
    [property: JsonPropertyName("bookId")] int BookId
);

public record EgzemplarzUpdateDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("bookId")] int BookId
);

