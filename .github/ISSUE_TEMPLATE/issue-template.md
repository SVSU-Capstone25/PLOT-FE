---
name: Issue Template
about: Describe this issue template's purpose here.
title: ''
labels: ''
assignees: ''

---

## 🎯 Acceptance Criteria

> [!NOTE]
> The following are developer friendly images and comments to explain what will be accepted as done.

Add images for designs and more developer specifics for them to follow. (DELETE THIS COMMENT)

(Image of User dashboard with "..." being hovered over, showing "Edit" and "Delete" options.)

After the "Delete" button is clicked it should show a modal that says something like "Are you sure you want to delete this user?" with a "Confirm" and "Cancel" button. 

If the "Cancel" button is clicked, it should close the modal and return the user to an unaltered user dashboard.

If the "Confirm" button is clicked, it should hit the `deleteUser` endpoint on the backend and supply the user ID in which to delete.

After the user is deleted, the table should refresh showing that the user was deleted and an alert/toast should show that says something like "User has been deleted."

...(DELETE THIS PLACEHOLDER CONTENT)

## 🧪 Testing

> [!NOTE]
> The following are what the E2E tests expect once development is done.

**Given** the ellipses 

**When** a user clicks on it 

**Then** a menu pops up with "Edit" and "Delete" buttons.

---

(IF THE ABOVE FORMAT DOESN'T WORK YOU CAN DEFAULT TO SOMETHING LIKE THIS)

When a user clicks the "Delete" button I **EXPECT**:
- A modal to pop up that says "Are you sure you want to delete this user?" with a "Confirm" and "Cancel" button.

---

... (DELETE THIS PLACEHOLDER CONTENT)

## 🔗 Links & Notes

> [!NOTE]
> The following are links and any other notes to aid other developers and leads.

- [Google](google.com)
- I ran into an issue when implementing this and found this workaround...
- ... (DELETE THIS PLACEHOLDER CONTENT)
