---
$attribute: CustomPage("Guides/Design Tokens")
---

## Design Tokens

Design tokens are the shared values that define your design system — colors, spacing, typography, and other visual properties. Using tokens ensures consistency across all components.

### Token categories

#### Colors

| Token | Value | Usage |
|-------|-------|-------|
| `--color-primary` | `#1ea7fd` | Primary actions, links |
| `--color-secondary` | `#6c757d` | Secondary actions |
| `--color-success` | `#28a745` | Success states |
| `--color-danger` | `#dc3545` | Error states, destructive actions |
| `--color-warning` | `#ffc107` | Warning messages |

#### Spacing

All spacing uses a 4px base unit:

| Token | Value |
|-------|-------|
| `--space-xs` | `4px` |
| `--space-sm` | `8px` |
| `--space-md` | `16px` |
| `--space-lg` | `24px` |
| `--space-xl` | `32px` |

#### Typography

| Token | Value |
|-------|-------|
| `--font-family` | `'Nunito Sans', sans-serif` |
| `--font-size-sm` | `12px` |
| `--font-size-md` | `14px` |
| `--font-size-lg` | `16px` |
| `--font-weight-normal` | `400` |
| `--font-weight-bold` | `700` |

### Using tokens in components

Apply tokens via CSS custom properties in your component styles:

```css
.my-button {
    background-color: var(--color-primary);
    padding: var(--space-sm) var(--space-md);
    font-family: var(--font-family);
    font-size: var(--font-size-md);
    border-radius: 4px;
}
```

This approach makes it easy to update your entire design system by changing token values in one place.
