<script setup>
  import { ref, watchEffect } from 'vue';
  import UserFormModal from './UserFormModal.vue';
  import SearchAndFilter from './SearchAndFilter.vue';
  import Pagination from './Pagination.vue';
  // import { orderBy } from 'lodash';
  import { useRoute } from 'vue-router';

  const users = ref([]);
  const mode = ref(null);
  const modal = ref(false);
  const modalData = ref({});
  const total = ref(0);
  const route = useRoute();

  const filterData = ref({
    name: route.query.name || '',
    minAge: route.query.minAge || 0,
    maxAge: route.query.maxAge || 0,
    manager: route.query.manager || '',
    type: route.query.type || '',
    email: route.query.email || '',
    page: route.query.page,
    limit: route.query.limit
    //sort: route.query.sort || ''
  });

  const fetchData = async () => {
    try {
      const response = await fetch(
        `/api/User?managerName=${filterData.value.manager}&` +
          `minAge=${filterData.value.minAge}&maxAge=${filterData.value.maxAge}&` +
          `name=${filterData.value.name}&email=${filterData.value.email}&` +
          `type=${filterData.value.type}&page=${route.query.page}&limit=${route.query.limit}` //&sort=${filterData.value.sort}`
      );
      const { users: data, count } = await response.json();
      users.value = data;
      total.value = count;
    } catch (error) {
      console.log('Error fetching all users: ' + error);
    }
  };

  watchEffect(async () => {
    await fetchData();
  });

  const handleClick = (type, index) => {
    mode.value = type;
    modalData.value = users.value[index];
    modal.value = true;
  };

  const fields = [
    'firstName',
    'lastName',
    'age',
    'email',
    'birthday',
    'phoneNum',
    'managerName',
    'account'
  ];

  const headers = [
    'First Name',
    'Last Name',
    'Age',
    'Email',
    'Birthday',
    'Phone Number',
    'Manager',
    'Account'
  ];

  const displayData = (user, field) => {
    if (field === 'account') {
      return user[field].type;
    } else if (field === 'birthday') {
      return new Date(user[field]).toLocaleDateString('en-US', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
      });
    }
    return user[field];
  };

  // const sortColumn = (col, index) => {
  //   col = fields[index];
  //   reverse.value[col] = !reverse.value[col];
  //   if (col === 'account') {
  //     users.value = orderBy(users.value, ['account.type'], [reverse.value[col] ? 'desc' : 'asc']);
  //   } else {
  //     users.value = orderBy(users.value, [col], [reverse.value[col] ? 'desc' : 'asc']);
  //   }
  // };
</script>

<template>
  <main class="container">
    <h1 class="mb-3">All Users</h1>
    <SearchAndFilter v-model="filterData" />
    <div v-if="users && users.length > 0" class="table-responsive">
      <table class="table table-striped table-hover table-bordered table-dark">
        <thead>
          <tr>
            <th v-for="field in headers" :key="field">
              {{ field }}
            </th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(user, index) of users" :key="user.id">
            <td v-for="field in fields" :key="field">{{ displayData(user, field) }}</td>
            <td>
              <button
                type="button"
                class="btn btn-primary"
                data-bs-target="#formModal"
                @click="() => handleClick('edit', index)"
              >
                Edit
              </button>
              <button
                type="button"
                class="btn btn-danger"
                @click="() => handleClick('delete', index)"
              >
                Delete
              </button>
            </td>
          </tr>
        </tbody>
      </table>
      <UserFormModal
        v-if="modal"
        :mode="mode"
        :data="modalData"
        v-model="modal"
        @onUpdate="() => fetchData()"
      />
      <Pagination
        v-model:page="filterData.page"
        v-model:limit="filterData.limit"
        v-model:count="total"
      />
    </div>
  </main>
</template>