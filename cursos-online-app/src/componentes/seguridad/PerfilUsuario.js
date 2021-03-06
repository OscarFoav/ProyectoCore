import {
  Container,
  Grid,
  TextField,
  Typography,
  Button,
} from "@material-ui/core";
import React, { useState, useEffect } from "react";
import {
  actualizarUsuario,
  obtenerUsuarioActual,
} from "../../actions/UsuarioAction";
import { useStateValue } from "../../contexto/store";
import style from "../Tool/Style";

const PerfilUsuario = () => {
  const [{ sesionUsuario }, dispatch] = useStateValue();
  const [usuario, setUsuario] = useState({
    nombreCompleto: "",
    email: "",
    password: "",
    confirmarPassword: "",
    username: "",
  });

  const ingresarValoresMemoria = (e) => {
    const { name, value } = e.target;
    setUsuario((anterior) => ({
      ...anterior,
      [name]: value,
    }));
  };

  useEffect(() => {
    obtenerUsuarioActual(dispatch).then((response) => {
      console.log(
        "Esta es la data del objeto response del usuario actual",
        response
      );
      setUsuario(response.data);
    });
  }, []);

  const guardarUsuario = (e) => {
    e.preventDefault();
    actualizarUsuario(usuario).then((response) => {
      if (response.status === 200) {
        dispatch({
          type: "OPEN_SNACKBAR",
          openMensaje: {
            open: true,
            mensaje: "Se han guardado los cambios de forma correcta en el Perfil Usuario"
          }
        })
        window.localStorage.setItem("token_seguridad", response.data.token);
      } else {
        dispatch({
          type: "OPEN_SNACKBAR",
          openMensaje: {
            open: true,
            mensaje:
              "Error al intentar guardar en : " +
              Object.keys(response.data.errors),
          },
        });
      }
    });
  };

  return (
    <Container component="main" maxWidth="md" justify="center">
      <div style={style.paper}>
        <Typography component="h1" variant="h5">
          Perfil de Usuario
        </Typography>
      </div>
      <form style={style.form}>
        <Grid container spacing={2}>
          <Grid item xs={12} md={12}>
            <TextField
              name="nombreCompleto"
              value={usuario.nombreCompleto}
              onChange={ingresarValoresMemoria}
              variant="outlined"
              fullWidth
              label="Introducir nombre y apellidos"
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <TextField
              name="email"
              value={usuario.email}
              onChange={ingresarValoresMemoria}
              variant="outlined"
              fullWidth
              label="Introducir email"
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <TextField
              name="username"
              value={usuario.username}
              onChange={ingresarValoresMemoria}
              variant="outlined"
              fullWidth
              label="Introducir username"
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <TextField
              name="password"
              value={usuario.password}
              onChange={ingresarValoresMemoria}
              type="password"
              variant="outlined"
              fullWidth
              label="Introducir password"
            />
          </Grid>
          <Grid item xs={12} md={6}>
            <TextField
              name="confirmarPassword"
              value={usuario.confirmarPassword}
              onChange={ingresarValoresMemoria}
              type="password"
              variant="outlined"
              fullWidth
              label="Confirmar password"
            />
          </Grid>
        </Grid>
        <Grid container justify="center">
          <Grid item xs={12} md={6}>
            <Button
              type="submit"
              onClick={guardarUsuario}
              fullWidth
              variant="contained"
              size="large"
              color="primary"
              style={style.submit}
            >
              Guardar Datos
            </Button>
          </Grid>
        </Grid>
      </form>
    </Container>
  );
};

export default PerfilUsuario;
