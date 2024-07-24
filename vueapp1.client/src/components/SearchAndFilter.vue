<script setup>
  //import { useQueryClient } from '@tanstack/vue-query';
  import { useRoute, useRouter } from 'vue-router';
  import AutoComplete from './AutoComplete.vue';
  import { computed } from 'vue';

  const formData = defineModel({ type: Object, required: true });
  const router = useRouter();
  const route = useRoute();
  //const queryClient = useQueryClient();

  const values = {
    max: 100,
    min: 0
  };

  const reset = () => {
    formData.value = {
      name: '',
      minAge: values.min,
      maxAge: values.max,
      managerName: '',
      type: '',
      page: 1,
      limit: route.query.limit
    };
    router.push({ name: route.name, query: { page: 1, limit: route.query.limit } });
    //queryClient.invalidateQueries({ queryKey: ['users'], refetchType: 'all' });
  };

  const maxArray = computed(() => {
    const arr = [];
    for (let i = formData.value.minAge; i <= values.max; i++) {
      arr.push(i);
    }
    return arr;
  });

  const minArray = computed(() => {
    const arr = [];
    for (let i = values.min; i <= formData.value.maxAge; i++) {
      arr.push(i);
    }
    return arr;
  });

  const updateQuery = (val, key) => {
    formData.value[key] = val;
    const query = Object.assign({}, route.query);
    if ((key == 'maxAge' && formData.value[key] == values.max) || !formData.value[key]) {
      delete query[key];
    } else {
      query[key] = val;
    }
    router.replace({ query });
  };
</script>

<template>
  <div>
    <div class="input-group mb-3">
      <input
        type="text"
        class="form-control"
        placeholder="Search"
        :value="formData.name"
        @input="(e) => updateQuery(e.target.value, 'name')"
      />
      <span class="input-group-text"><i class="bi bi-search"></i></span>
      <div class="dropdown z-0">
        <button
          type="button"
          class="ms-3 btn btn-secondary"
          data-bs-toggle="dropdown"
          data-bs-auto-close="false"
        >
          <i class="bi bi-list"></i>
        </button>
        <div class="dropdown-menu dropdown-menu-end p-4 text-bg-dark filter-width">
          <div class="row mb-3">
            <div class="col">
              <label class="form-label">Min Age</label>
              <select
                class="form-select"
                :value="formData.minAge"
                @change="(e) => updateQuery(e.target.value, 'minAge')"
              >
                <option v-for="(num, index) in minArray" :key="'minAge-' + index">{{ num }}</option>
              </select>
            </div>
            <div class="col">
              <label class="form-label">Max Age</label>
              <select
                class="form-select"
                :value="formData.maxAge"
                @change="(e) => updateQuery(e.target.value, 'maxAge')"
              >
                <option v-for="(num, index) in maxArray" :key="'maxAge-' + index">{{ num }}</option>
              </select>
            </div>
          </div>
          <AutoComplete name="managerName" @update="(val) => updateQuery(val, 'managerName')" />
          <div class="mb-3">
            <label class="form-label">Employee Type</label>
            <select
              class="form-select"
              :value="formData.type"
              @change="(e) => updateQuery(e.target.value, 'type')"
            >
              <option value="">Select...</option>
              <option>Admin</option>
              <option>Manager</option>
              <option>Employee</option>
            </select>
          </div>
          <div class="d-flex justify-content-center">
            <button type="button" class="btn btn-primary" @click="reset">Reset</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
  .filter-width {
    width: 25rem;
  }

  @media (width <= 576px) {
    .filter-width {
      width: 20rem;
    }
  }
</style>
