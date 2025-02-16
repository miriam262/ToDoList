import React from "react";
import Sessions from "./Sessions";
import Box from "@mui/material/Box";
import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import service from "../service";
import { useEffect, useState } from "react";
import "../styles/index.css";

export default function Private() {
  const [newTodo, setNewTodo] = useState("");
  const [todos, setTodos] = useState([]);

  async function getTodos() {
    const todos = await service.getTasks();
    alert(todos);
    setTodos(todos);
  }

  async function createTodo(e) {
    e.preventDefault();
    await service.addTask(newTodo);
    setNewTodo("");
    await getTodos();
  }

  async function updateCompleted(todo, isComplete) {
    await service.setCompleted(todo.id, isComplete);
    await getTodos();
  }

  async function deleteTodo(id) {
    await service.deleteTask(id);
    await getTodos();
  }

  useEffect(() => {
    const token = localStorage.getItem("access_token");
    alert(token);
    if (token) {
      getTodos();
    }
  }, []);
  

  return (
    <Box
      sx={{
        bgcolor: "background.paper",
        pt: 8,
        pb: 6,
      }}
    >
      <Container maxWidth="md">
        <Typography
          component="h1"
          variant="h2"
          align="center"
          color="text.primary"
          gutterBottom
        >
          דף פרטי
        </Typography>
        <Typography
          variant="h5"
          align="center"
          color="text.secondary"
          paragraph
        >
          את המידע בדף הזה יכולים לראות רק משתמשים מחוברים.
        </Typography>
        <Typography
          component="h5"
          variant="h5"
          align="center"
          color="text.primary"
          gutterBottom
        >
          מידע על התחברויות המשתמש
        </Typography>
        {<Sessions />}

        {localStorage.getItem("access_token") && (
          <section className="todoapp">
            <header className="header">
              <h1 style={{ backgroundColor: "#f0f0f0", padding: "10px" }}>todos</h1><br></br>
              <form onSubmit={createTodo}>
                <input
                  className="new-todo"
                  placeholder="Well, let's take on the day"
                  value={newTodo}
                  onChange={(e) => setNewTodo(e.target.value)}
                  style={{
                    backgroundColor: "#ffffff",
                    marginTop: "40px", 
                    padding: "10px",
                    width: "100%",
                  }}
                />
              </form>
            </header>
            <section className="main" style={{ display: "block" }}>
              <ul className="todo-list">
                {todos.map((todo) => {
                  return (
                    <li
                      className={todo.isComplete ? "completed" : ""}
                      key={todo.id}
                    >
                      <div className="view">
                        <input
                          className="toggle"
                          type="checkbox"
                          checked={todo.isComplete}
                          onChange={(e) =>
                            updateCompleted(todo, e.target.checked)
                          }
                        />
                        <label className="oiuytrertyuio">{todo.name}</label>
                        <button
                          className="destroy"
                          onClick={() => deleteTodo(todo.id)}
                        ></button>
                      </div>
                    </li>
                  );
                })}
              </ul>
            </section>
          </section>
        )}
      </Container>
    </Box>
  );
}