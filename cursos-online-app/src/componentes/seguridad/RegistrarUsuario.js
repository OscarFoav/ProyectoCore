import React,  {useState} from 'react';
import {Container, Grid, TextField, Typography, Button} from '@material-ui/core';
import style from '../Tool/Style';
import { registrarUsuario } from '../../actions/UsuarioAction';


const RegistrarUsuario = () => {
    const [usuario, setUsuario] = useState ({
        NombreCompleto : '', 
        Email : '',
        Username : '',
        Password : '',
        ConfirmarPassword : ''
    })

    const IngresarValoresMemoria = e => {
        const {name, value} = e.target;
        setUsuario(anterior => ({
            ...anterior, 
            [name] : value
        }))
    }

    const registrarUsuarioBoton = e => {
        e.preventDefault();
        // console.log('Imprime los valores de memoria temporal de usuario', usuario);
        registrarUsuario(usuario).then(response => {
            console.log('Se ha registrado el usuario de forma correcta', response);
            window.localStorage.setItem("token_seguridad", response.data.token);
        });
    }

    return (
        <Container component="main" maxWidth="md" justify="center">
            <div style={style.paper}>
                <Typography component="h1" variant="h5" >
                    Registro de Usuario
                </Typography>
                <form style={style.form}>
                    <Grid container spacing={2}>
                        <Grid item xs={12} md={12}>
                            <TextField name="NombreCompleto" value={usuario.NombreCompleto} onChange={IngresarValoresMemoria} variant="outlined" fullWidth label="Escriba nombre y apellidos" />
                        </Grid>                        
                        <Grid item xs={12} md={6}>
                            <TextField name="Email" value={usuario.Email} onChange={IngresarValoresMemoria}   variant="outlined" fullWidth label="Escriba su email" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="Username" value={usuario.Username} onChange={IngresarValoresMemoria}  variant="outlined" fullWidth label="Escriba su username" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="Password" value={usuario.Password} onChange={IngresarValoresMemoria}  type="password" variant="outlined" fullWidth label="Escriba su password" />
                        </Grid>
                        <Grid item xs={12} md={6}>
                            <TextField name="ConfirmarPassword" value={usuario.ConfirmarPassword} onChange={IngresarValoresMemoria}  type="password" variant="outlined" fullWidth label="Confirme su password" />
                        </Grid>
                        <Grid container justify="center">
                            <Grid item xs={12} mf={6}>
                                <Button type="submit" onClick={registrarUsuarioBoton} fullWidth variant="contained" color="primary" size="large" style={style.submit}>
                                    Enviar
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>
                </form>
            </div>
        </Container>
    );
}

export default RegistrarUsuario;
