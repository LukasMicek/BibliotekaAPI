using System.Text.Json.Serialization;

namespace BibliotekaAPI.Dtos;

public record KsiążkaDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("year")] int Year,
    [property: JsonPropertyName("author")] AutorDto Author
);

public record KsiążkaCreateDto(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("year")] int Year,
    [property: JsonPropertyName("authorId")] int AuthorId
);

public record KsiążkaUpdateDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("year")] int Year,
    [property: JsonPropertyName("authorId")] int AuthorId
);
