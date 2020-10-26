import React, { useEffect, useState } from "react";
import { ThemeProvider as MuithemeProvider } from "@material-ui/core/styles";
import theme from "./theme/theme";
import RegistrarUsuario from "./componentes/seguridad/RegistrarUsuario";
import Login from "./componentes/seguridad/Login";
import PerfilUsuario from "./componentes/seguridad/PerfilUsuario";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";
import { Grid } from "@material-ui/core";
import AppNavBar from "./componentes/navegacion/AppNavBar";
import { useStateValue } from "././contexto/store";
import { obtenerUsuarioActual } from "./actions/UsuarioAction";

function App() {
  const [{sesionUsuario}, dispatch] = useStateValue();
  const [ iniciaApp, setIniciaApp] = useState(false);

  useEffect(() => {
    if(!iniciaApp){
      obtenerUsuarioActual(dispatch).then(response => {
        setIniciaApp(true);
      }).catch(error =>{
        setIniciaApp(true);
      })
    }

  }, [iniciaApp])


  return (
    <Router>
      <MuithemeProvider theme={theme}>
        <AppNavBar />
        <Grid container>
          <switch>
            <Route exact path="/auth/login" component={Login} />
            <Route exact path="/auth/registrar" component={RegistrarUsuario} />
            <Route exact path="/auth/perfil" component={PerfilUsuario} />
            <Route exact path="/" component={PerfilUsuario} />
          </switch>
        </Grid>
      </MuithemeProvider>
    </Router>
  );
}

export default App;
