<script setup>
  import { inject, ref, watchEffect } from 'vue';
  import { useRoute, useRouter } from 'vue-router';
  import Pagination from './Pagination.vue';
  import ReviewFormModal from './ReviewFormModal.vue';

  const { user } = inject('user');
  const performances = ref([]);
  const total = ref(0);
  const route = useRoute();
  const router = useRouter();
  const params = ref({
    type: route.query.type,
    page: route.query.page,
    limit: route.query.limit
  });
  const modal = ref(false);

  const fetchPerformances = async () => {
    try {
      const response = await fetch(
        `/api/Performance?user=${user?.id}&type=${params.value.type}&page=${params.value.page}&limit=${params.value.limit}`
      );
      const { performances: data, count } = await response.json();
      performances.value = data;
      total.value = count;
    } catch (error) {
      console.log('Error: something went wrong while fetching performance reviews. ' + error);
    }
  };

  watchEffect(async () => {
    await fetchPerformances();
  });
</script>

<template>
  <div class="container-fluid">
    <div class="container-fluid d-flex justify-content-around m-4 p-4">
      <div></div>
      <div class="d-flex flex-shrink-1">
        <button
          type="button"
          class="btn btn-outline-light rounded-start-pill btn-lg"
          :class="{ active: params.type === 'self' }"
          @click="
            () => {
              params.type = 'self';
              const query = Object.assign({}, route.query);
              query.type = 'self';
              router.replace({ query });
            }
          "
        >
          Self Reviews
        </button>
        <button
          type="button"
          class="btn btn-outline-light rounded-end-pill btn-lg"
          :class="{ active: params.type === 'manager' }"
          @click="
            () => {
              params.type = 'manager';
              const query = Object.assign({}, route.query);
              query.type = 'manager';
              router.replace({ query });
            }
          "
        >
          Manager Reviews
        </button>
      </div>
      <button
        v-if="params.type === 'self'"
        class="btn btn-primary rounded-circle btn-lg"
        @click="() => (modal = true)"
        data-bs-toggle="tooltip"
        data-bs-placement="top"
        data-bs-title="Add Review"
      >
        <i class="bi bi-plus"></i>
      </button>
    </div>
    <div
      class="row row-cols-lg-4 row-cols-xl-5 row-cols-xxl-6 row-cols-md-3 row-cols-1 row-cols-sm-2 g-4 mb-4"
    >
      <div class="col" v-for="(performance, index) in performances" :key="performance.id">
        <div class="card">
          <div class="card-header">
            {{ new Date(performance.createdAt).toLocaleString('en-US') }}
          </div>
          <div class="card-body">
            <div class="card-text mb-3">
              Rating:
              <div class="d-inline-flex flex-wrap ms-1 text-secondary">
                <span
                  :class="{ 'text-black': n <= performance.rating }"
                  v-for="n in 5"
                  :key="performance.id + '-star-' + n"
                >
                  &#9733;
                </span>
              </div>
            </div>
            <div class="accordion">
              <div class="accordion-item">
                <div class="accordion-header">
                  <button
                    class="accordion-button collapsed"
                    type="button"
                    data-bs-toggle="collapse"
                    :data-bs-target="'#performance-goals-' + index"
                  >
                    Goals
                  </button>
                </div>
                <div :id="'performance-goals-' + index" class="accordion-collapse collapse">
                  <div class="accordion-body">
                    <ul>
                      <li
                        v-for="(goal, idx) in performance.goals"
                        :key="performance.id + '-goals-' + idx"
                      >
                        {{ goal }}
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
              <div class="accordion-item">
                <div class="accordion-header">
                  <button
                    class="accordion-button collapsed"
                    type="button"
                    data-bs-toggle="collapse"
                    :data-bs-target="'#things-to-improve-' + index"
                  >
                    Things to Improve
                  </button>
                </div>
                <div :id="'things-to-improve-' + index" class="accordion-collapse collapse">
                  <div class="accordion-body">
                    <ul>
                      <li
                        v-for="(improve, idx) in performance.improve"
                        :key="performance.id + '-improve-' + idx"
                      >
                        {{ improve }}
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
              <div class="accordion-item">
                <div class="accordion-header">
                  <button
                    class="accordion-button collapsed"
                    type="button"
                    data-bs-toggle="collapse"
                    :data-bs-target="'#things-did-well-' + index"
                  >
                    Things did well
                  </button>
                </div>
                <div :id="'things-did-well-' + index" class="accordion-collapse collapse">
                  <div class="accordion-body">
                    <ul>
                      <li
                        v-for="(well, idx) in performance.well"
                        :key="performance.id + '-well-' + idx"
                      >
                        {{ well }}
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <Pagination v-model:limit="params.limit" v-model:page="params.page" v-model:count="total" />
    <ReviewFormModal
      v-if="modal"
      :type="params.type"
      v-model="modal"
      @onCreated="async () => await fetchPerformances()"
    />
  </div>
</template>