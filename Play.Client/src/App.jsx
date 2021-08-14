import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Catalog } from './components/Catalog';
import { Inventory } from './components/Inventory';
import { Users } from './components/Users';
import AuthorizeRoute from './api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './api-authorization/ApiAuthorizationRoutes';
import { AuthorizationPaths } from './constants/ApiAuthorizationConstants';
import { ApplicationPaths } from './constants/Constants';

import './App.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path="/" component={Home} />
        <AuthorizeRoute path={ApplicationPaths.CatalogPath} component={Catalog} />
        <AuthorizeRoute path={ApplicationPaths.InventoryPath} component={Inventory} />
        <AuthorizeRoute path={ApplicationPaths.UsersPath} component={Users} />
        <Route path={AuthorizationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}
