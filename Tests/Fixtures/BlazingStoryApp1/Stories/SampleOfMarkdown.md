---
$attribute: CustomPage("Examples/Sample of Markdown")
---

# Hello, World!

## Section 1

### Sub title 1

This is a simple example of a document with various code snippets.

```html
<!-- This is a comment -->
<h1 id="hello-world">hello world</h1>
```

```typescript
// This is a comment
const greet = (name: string): string => {
  return `Hello, ${name}!`;
};

class Person {
  constructor(name: string) {
    this.name = name;
  }

  public greet(): string {
    return `Hello, ${this.name}` + "!";
  }
}
```

```csharp
// This is a comment
public class Person {
  public string Name { get; set; }

  public Person(name) {
    this.Name = name;
  }

  public string Greet() {
    return $"Hello, {this.name}" + "!";
  }
}
```

```json
{
  "string": "string",
  "number": 123,
  "boolean": true,
  "array": [1, 2, 3],
  "object": {
    "key": "value"
  },
  "null": null
}
```