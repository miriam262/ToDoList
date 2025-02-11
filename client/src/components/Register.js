import * as React from "react";
import Avatar from "@mui/material/Avatar";
import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
import FormControlLabel from "@mui/material/FormControlLabel";
import Checkbox from "@mui/material/Checkbox";
import Link from "@mui/material/Link";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import Typography from "@mui/material/Typography";
import { useNavigate } from "react-router-dom";
import Service from "../service";
import { useState } from "react";
import Container from "@mui/material/Container";

export default function Register() {
 const [Id, setId] = useState("0");
   const [username, setUsername] = useState("פלוני");
   const [password, setPassword] = useState("123456");

  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();
    await Service.register(Id, username, password);
    navigate("/private", { replace: true });
  };

  return (
    <Container maxWidth="xs">
      <Box
        sx={{
          marginTop: 8,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Avatar sx={{ m: 1, bgcolor: "secondary.main" }}>
          <LockOutlinedIcon />
        </Avatar>
        <Typography component="h1" variant="h5">
          הרשמה
        </Typography>
        <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
         <TextField
                     margin="normal"
                     required
                     fullWidth
                     id="number"
                     label="תעודת זהות"
                     name="Id"
                     autoComplete="Id"
                     autoFocus
                     onChange={(event) => setId(event.target.value)}
                   />
                   <TextField
                   margin="normal"
                   required
                   fullWidth
                   id="username"
                   label="שם משתמש"
                   name="name"
                   autoComplete="name"
                   autoFocus
                   onChange={(event) => setUsername(event.target.value)}
                 />
                   <TextField
                     margin="normal"
                     required
                     fullWidth
                     name="password"
                     label="סיסמה"
                     type="password"
                     id="password"
                     autoComplete="current-password"
                     onChange={(event) => setPassword(event.target.value)}
                   />
          <FormControlLabel
            control={<Checkbox value="remember" color="primary" />}
            label="זכור אותי"
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            sx={{ mt: 3, mb: 2 }}
          >
            הרשמה
          </Button>
          <Grid container>
            <Grid item>
              <Link href="/login" variant="body2">
                {"יש לך כבר חשבון? להתחברות"}
              </Link>
            </Grid>
          </Grid>
        </Box>
      </Box>
    </Container>
  );
}
