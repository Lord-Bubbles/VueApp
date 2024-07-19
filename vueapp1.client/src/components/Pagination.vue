<!-- eslint-disable vue/multi-word-component-names -->
<script setup>
  import { computed } from 'vue';
  import { useRoute, useRouter } from 'vue-router';

  const limit = defineModel('limit', {
    required: true,
    type: Number
  });
  const page = defineModel('page', {
    required: true,
    type: Number
  });
  const count = defineModel('count', {
    required: true,
    type: Number
  });

  const route = useRoute();
  const router = useRouter();

  const DOTS = '...';
  const siblings = 1;

  const range = (from, to) => {
    let length = to - from + 1;
    return Array.from({ length }, (_, idx) => idx + from);
  };

  const paginationRange = computed(() => {
    const totalPageCount = Math.ceil(count.value / limit.value);

    // Number of pages to display in pagination bar is determined as
    // siblingCount + firstPage + lastPage + currentPage + 2*DOTS
    const totalPageNumbers = siblings + 5;

    // Case 1:
    // If the number of pages is less than the page numbers we want to show in
    // our paginationComponent, we return the range [1..totalPageCount]
    if (totalPageNumbers >= totalPageCount) {
      return range(1, totalPageCount);
    }

    // Calculate left and right sibling index and make sure they are within
    // range 1 and totalPageCount
    const leftSiblingIndex = Math.max(page.value - siblings, 1);
    const rightSiblingIndex = Math.min(page.value + siblings, totalPageCount);

    // We do not show dots just when there is just one page number to be
    // inserted between the extremes of sibling and the page limits i.e 1 and
    // totalPageCount. Hence we are using leftSiblingIndex > 2 and
    // rightSiblingIndex < totalPageCount - 2
    const showLeftDots = leftSiblingIndex > 2;
    const showRightDots = rightSiblingIndex < totalPageCount - 2;

    const firstPageIndex = 1;
    const lastPageIndex = totalPageCount;

    // For the side without dots, always show this number of pages in the
    // pagination bar
    const itemCount = 3 + 2 * siblings;

    // Case 2: No left dots to show, but rights dots to be shown
    if (!showLeftDots && showRightDots) {
      let leftRange = range(1, itemCount);
      return [...leftRange, DOTS, totalPageCount];
    }

    // Case 3: No right dots to show, but left dots to be shown
    if (showLeftDots && !showRightDots) {
      let rightRange = range(totalPageCount - itemCount + 1, totalPageCount);
      return [firstPageIndex, DOTS, ...rightRange];
    }

    // Case 4: Both left and right dots to be shown
    let middleRange = range(leftSiblingIndex, rightSiblingIndex);
    return [firstPageIndex, DOTS, ...middleRange, DOTS, lastPageIndex];
  });

  const updatePageQuery = (num) => {
    page.value = num;
    const query = Object.assign({}, route.query);
    query.page = num;
    router.replace({ query });
  };
</script>

<template>
  <nav v-if="paginationRange.length > 1">
    <ul class="pagination justify-content-center">
      <li class="page-item" :class="{ disabled: page == 1 }">
        <button class="page-link" type="button" @click="updatePageQuery(page - 1)">&laquo;</button>
      </li>
      <li
        class="page-item"
        :class="{ active: page == num, disabled: num === DOTS, 'text-bg-info': page == num }"
        v-for="(num, index) in paginationRange"
        :key="num + '-' + index"
      >
        <button class="page-link" type="button" v-if="num === DOTS">&#8230;</button>
        <button class="page-link" type="button" @click="updatePageQuery(num)" v-else>
          {{ num }}
        </button>
      </li>
      <li
        class="page-item"
        :class="{ disabled: page == paginationRange[paginationRange.length - 1] }"
      >
        <button class="page-link" type="button" @click="updatePageQuery(page + 1)">&raquo;</button>
      </li>
    </ul>
  </nav>
</template>
